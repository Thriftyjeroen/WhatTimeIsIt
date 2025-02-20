using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    public Camera playerCamera; // the camera who follows the player 
    public float walkSpeed = 12f; // speed of how fast the player is while walking 
    public float runSpeed = 16f; // speed of how fast player is while running 
    public float jumpPower = 15f; // the power how high the players comes when jumping 
    public float gravity = 25f; // determined the gravity 
    public float lookSpeed = 2f; // how fast the camera is while looking around 
    public float lookXLimit = 45f; // how far you can look on X axis
    public float defaultHeight = 2f; // the normal height of the player 
    public float crouchHeight = 1f; // the height of the player while crouching 
    public float crouchSpeed = 3f; // how fast you can crouch 

    private State state; // Current state of the player (Normal, HookshotThrown, HookshotFlyingPlayer)
    private float hookshotSize; // Current length of the hookshot (visual and functional)
    private float rotationX = 0f; // Vertical camera rotation value for clamping and movement
    private bool canMove = true; // Flag to determine if the player can move (used to disable movement temporarily)
    private Vector3 moveDirection = Vector3.zero; // The direction the player is moving in
    private Vector3 hookshotPosition; // The target position of the hookshot (where it's anchored)
    private Vector3 characterVelocityMomentum; // Momentum applied to the character for smooth movement transitions
    private CharacterController characterController; // Reference to the CharacterController component managing player movement
    private CameraFov cameraFov; // Reference to a script managing camera field of view changes
    private Collider playerCollider; // Reference to the player's collider for interactions and physics calculations



    [SerializeField] GameObject hookPullSound;
    private GameObject soundObject;
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
        if (Time.timeScale == 0f)
        {
            return;
        }
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

                soundObject = Instantiate(hookPullSound);

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

    // Handle the Movement of the Hookshot 
    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDirection = (hookshotPosition - transform.position).normalized; // calculated the direction of the hookshot 

        float hookshotSpeedMin = 30f; // min speed of the hookshot 
        float hookshotSpeedMax = 60f; // max speed of the hookshot 
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax); // the speed of the hookshot
        float hookshotSpeedMult = 2f; // mult of the hookshotspeed 

        // Move player along hookshot direction
        characterController.Move(hookshotDirection * hookshotSpeed * hookshotSpeedMult * Time.deltaTime);

        // Dynamically adjust the rope's length
        float distanceToHookshot = Vector3.Distance(transform.position, hookshotPosition);
        hookshotTransform.localScale = new Vector3(1, 1, distanceToHookshot);

        // Check if player has reached the target
        float reachedHookshotDistance = 2f;
        if (distanceToHookshot < reachedHookshotDistance)
        {
            Destroy(soundObject); // destroyed the soundobject 

            state = State.Normal; // set state to normal 
            canMove = true; // set the boolean to true so player can move
            ResetGravityForce(); // reset the gravity force 
            hookshotTransform.gameObject.SetActive(false);
            cameraFov.SetCameraFov(NORMAL_FOV);
            return;
        }

        // Cancel hookshot on secondary input
        if (TestInputDownHookshot())
        {
            Destroy(soundObject);

            characterVelocityMomentum = Vector3.zero;
            RetractHookshot();
            return;
        }

        // Handle jump cancelation
        if (TestInputJump())
        {
            Destroy(soundObject);

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


    // Retracts the hookshot and resets player state
    private void RetractHookshot()
    {
        state = State.Normal; // Reset to normal state
        ResetGravityForce();  // Stop gravity effects
        hookshotTransform.gameObject.SetActive(false); // Hide hookshot visuals
        cameraFov.SetCameraFov(NORMAL_FOV); // Reset camera FOV
    }


    // Resets vertical movement to zero (stops gravity effect).
    private void ResetGravityForce()
    {
        moveDirection.y = 0;
    }

    // Checks if the "F" key is pressed for hookshot action.
    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    // Checks if the "Space" key is pressed for jump action.
    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

}