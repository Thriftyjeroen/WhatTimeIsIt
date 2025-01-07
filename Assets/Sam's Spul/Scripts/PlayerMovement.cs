using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 16f;
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 hookshotPosition;
    private float rotationX = 0;
    private CharacterController characterController;
    private Vector3 characterVelocityMomentum;
    private float hookshotSize;

    private bool canMove = true;
    private State state;

    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer,
    }

    void Start()
    {
        state = State.Normal;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hookshotTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        // Apply momentum separately
        if (characterVelocityMomentum.magnitude > 0)
        {
            characterController.Move(characterVelocityMomentum * Time.deltaTime);

            // Gradually reduce momentum over time (damping)
            float momentumDamping = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDamping * Time.deltaTime;

            // Stop momentum if it becomes negligible
            if (characterVelocityMomentum.magnitude < 0.1f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        // Handle player rotation
        HandleMouseLook();

        // Handle state-based movement
        switch (state)
        {
            default:
            case State.Normal:
                HandleNormalMovement();
                break;
            case State.HookshotThrown:
                HandleHookshotTrow();
                HandleNormalMovement();

                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }
    }

    private void HandleMouseLook()
    {
        if (canMove)
        {
            // Rotate camera vertically
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Rotate player horizontally
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }


    private void HandleNormalMovement()
    {
        HandleHookshotStart();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (TestInputJump() && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleHookshotStart()
    {
        if (TestInputDownHookshot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
        }
    }

    private void HandleHookshotTrow()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 100f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;

        hookshotTransform.localScale = new Vector3(1,1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
        }
    }

    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDirection = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 30f;
        float hookshotSpeedMax = 60f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMult = 2f;

        // Move player along hookshot direction
        characterController.Move(hookshotDirection * hookshotSpeed * hookshotSpeedMult * Time.deltaTime);

        // Check if player reached the hookshot point
        float reachedHookshotDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotDistance)
        {
            state = State.Normal;
            ResetGravityForce();
            hookshotTransform.gameObject.SetActive(false);
            return;
        }

        // Cancel hookshot on secondary input
        if (TestInputDownHookshot())
        {
            state = State.Normal;
            ResetGravityForce();
            hookshotTransform.gameObject.SetActive(false);

            return;
        }

        // Handle jump cancelation
        if (TestInputJump())
        {
            // Reset vertical movement to avoid stacking forces
            moveDirection.y = 0;

            // Apply horizontal momentum based on hookshot direction
            float momentumExtraSpeed = 7f;
            Vector3 hookshotMomentum = hookshotDirection * momentumExtraSpeed * hookshotSpeed;

            // Add upward jump velocity
            float jumpSpeed = 15f; // Limit upward force
            Vector3 jumpMomentum = Vector3.up * jumpSpeed;

            // Combine horizontal and vertical momentum
            characterVelocityMomentum = hookshotMomentum + jumpMomentum;

            // Clamp total momentum magnitude
            float maxMomentum = 50f; // Adjust as needed
            characterVelocityMomentum = Vector3.ClampMagnitude(characterVelocityMomentum, maxMomentum);

            // Reset hookshot state
            state = State.Normal;
            ResetGravityForce();
            hookshotTransform.gameObject.SetActive(false);
        }
    }

    private void ResetGravityForce()
    {
        moveDirection.y = 0;
    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
