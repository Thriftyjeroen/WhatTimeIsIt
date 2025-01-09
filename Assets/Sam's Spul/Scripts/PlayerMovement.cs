using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    public Camera playerCamera;
    public float walkSpeed = 12f;
    public float runSpeed = 16f;
    public float jumpPower = 15f;
    public float gravity = 25f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private State state;
    private float hookshotSize;
    private float rotationX = 0f;
    private bool canMove = true;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 hookshotPosition;
    private Vector3 characterVelocityMomentum;
    private CharacterController characterController;
    private CameraFov cameraFov;
    private Collider playerCollider;

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
        cameraFov = playerCamera.GetComponent<CameraFov>();
        playerCollider = GetComponent<Collider>(); // Get the player's collider
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hookshotTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            characterVelocityMomentum = Vector3.zero;
        }

        if (characterVelocityMomentum.magnitude > 0)
        {
            characterController.Move(characterVelocityMomentum * Time.deltaTime);
            float momentumDamping = 0.5f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDamping * Time.deltaTime;

            if (characterVelocityMomentum.magnitude < 0.1f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        HandleMouseLook();

        switch (state)
        {
            default:
            case State.Normal:
                HandleNormalMovement();
                break;
            case State.HookshotThrown:
                HandleHookshotThrow();
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
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
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
            ResetGravityForce();
            moveDirection.y = movementDirectionY;
        }

        if (characterController.isGrounded)
        {
            characterVelocityMomentum = Vector3.zero;
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
            LayerMask mask = LayerMask.GetMask("PlayerLayer"); // Use your relevant layer
            mask = ~mask; // Invert the layer mask to exclude "Default" layer (assuming player collider is in "Default")

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, Mathf.Infinity, mask))
            {
                if (raycastHit.distance > 69f)
                {
                    return;
                }
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
        }
    }

    private void HandleHookshotThrow()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 175f;
        Vector3 hookshotDirection = (hookshotPosition - transform.position).normalized;

        // Perform a raycast to detect walls or obstacles, ignoring the player's collider
        LayerMask mask = LayerMask.GetMask("PlayerLayer");
        mask = ~mask; // Exclude player's own collider layer
        float raycastDistance = hookshotSize + hookshotThrowSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position, hookshotDirection, out RaycastHit hit, raycastDistance, mask))
        {
            hookshotPosition = hit.point; // Update target to collision point
            hookshotSize = Vector3.Distance(transform.position, hookshotPosition); // Adjust rope length
        }
        else
        {
            hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        }

        // Clamp the rope size to the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, hookshotPosition);
        hookshotSize = Mathf.Min(hookshotSize, distanceToTarget);

        // Update rope visuals
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        // Debug visuals for raycasting
        Debug.DrawLine(transform.position, transform.position + hookshotDirection * raycastDistance, Color.red, 0.1f);

        // Transition to flying state when rope reaches the target
        if (hookshotSize >= distanceToTarget)
        {
            state = State.HookshotFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
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

        // Dynamically adjust the rope's length
        float distanceToHookshot = Vector3.Distance(transform.position, hookshotPosition);
        hookshotTransform.localScale = new Vector3(1, 1, distanceToHookshot);

        // Check if player has reached the target
        float reachedHookshotDistance = 2f;
        if (distanceToHookshot < reachedHookshotDistance)
        {
            state = State.Normal;
            canMove = true;
            ResetGravityForce();
            hookshotTransform.gameObject.SetActive(false);
            cameraFov.SetCameraFov(NORMAL_FOV);
            return;
        }

        // Cancel hookshot on secondary input
        if (TestInputDownHookshot())
        {
            characterVelocityMomentum = Vector3.zero;
            RetractHookshot();
            return;
        }

        // Handle jump cancelation
        if (TestInputJump())
        {
            moveDirection.y = 0;
            float momentumExtraSpeed = 12f;
            Vector3 hookshotMomentum = hookshotDirection * momentumExtraSpeed * hookshotSpeed;
            float jumpSpeed = 50f;
            Vector3 jumpMomentum = Vector3.up * jumpSpeed;

            characterVelocityMomentum = hookshotMomentum + jumpMomentum;
            float maxMomentum = 66f;
            characterVelocityMomentum = Vector3.ClampMagnitude(characterVelocityMomentum, maxMomentum);

            RetractHookshot();
        }
    }


    private void RetractHookshot()
    {
        state = State.Normal;
        ResetGravityForce();
        hookshotTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
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