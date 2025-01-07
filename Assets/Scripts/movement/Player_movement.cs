using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    bool isGrounded;
    bool isWallRunning;
    bool isNearWall;
    float baseSpeed = 5f;
    float sprintMultiplier = 2f;
    float wallRunSpeed = 7f;
    float speed;
    float jumpForce = 7.5f;
    float airMultiplier = 0.5f;
    int jumpCount = 0;
    int maxJumps = 2;
    Rigidbody body;
    Vector3 moveDirection;
    Vector3 wallJumpDirection;
    string groundLayer = "ground";
    string wallLayer = "wall";

    [SerializeField] float groundDrag = 4f;
    [SerializeField] float airDrag = 1f;
    [SerializeField] float wallRunGravity = 2f;
    [SerializeField] float wallDistance = 1.5f;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        speed = baseSpeed;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        CheckWallRunning();

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            speed = baseSpeed * sprintMultiplier;
        }
        else
        {
            speed = baseSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (jumpCount < maxJumps)
            {
                Jump();
            }
        }

        if (isWallRunning && Input.GetKeyDown(KeyCode.Space))
        {
            WallRunJump();
        }

        SpeedControl();
        AdjustDrag();
    }

    void FixedUpdate()
    {
        if (isWallRunning)
        {
            WallRunMovement();
        }
        else
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            body.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else
        {
            body.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void WallRunMovement()
    {
        moveDirection = orientation.forward;

        Vector3 wallRunForce = moveDirection.normalized * wallRunSpeed + Vector3.down * wallRunGravity;

        body.AddForce(wallRunForce, ForceMode.Force);

        body.linearVelocity = new Vector3(body.linearVelocity.x, Mathf.Clamp(body.linearVelocity.y, -2f, 2f), body.linearVelocity.z);
    }

    private void WallRunJump()
    {
        body.useGravity = true;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            wallJumpDirection = transform.up + (-orientation.right);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            wallJumpDirection = transform.up + orientation.right;
        }
        body.AddForce(wallJumpDirection.normalized * jumpForce, ForceMode.Impulse);
        isWallRunning = false;
    }

    private void Jump()
    {
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(body.linearVelocity.x, 0, body.linearVelocity.z);

        if (flatVelocity.magnitude > speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * speed;
            body.linearVelocity = new Vector3(limitedVelocity.x, body.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void AdjustDrag()
    {
        if (isGrounded)
        {
            body.linearDamping = groundDrag;
        }
        else
        {
            body.linearDamping = airDrag;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        int groundLayerIndex = LayerMask.NameToLayer(groundLayer);
        int wallLayerIndex = LayerMask.NameToLayer(wallLayer);

        if (collision.gameObject.layer == groundLayerIndex)
        {
            isGrounded = true;
            jumpCount = 0;
            isWallRunning = false;
            body.useGravity = true;
        }
        else if (collision.gameObject.layer == wallLayerIndex)
        {
            StartWallRun();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        int groundLayerIndex = LayerMask.NameToLayer(groundLayer);
        int wallLayerIndex = LayerMask.NameToLayer(wallLayer);

        if (collision.gameObject.layer == groundLayerIndex)
        {
            isGrounded = false;
        }
        else if (collision.gameObject.layer == wallLayerIndex)
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        if (!isGrounded && isNearWall)
        {
            isWallRunning = true;
            body.useGravity = false;
        }
    }

    private void StopWallRun()
    {
        isWallRunning = false;
        body.useGravity = true;
    }

    private void CheckWallRunning()
    {
        RaycastHit hit;
        isNearWall = false;

        if (Physics.Raycast(transform.position, orientation.right, out hit, wallDistance) || Physics.Raycast(transform.position, -orientation.right, out hit, wallDistance))
        {
            isNearWall = true;
        }
        else
        {
            isNearWall = false;
        }

        if (!isNearWall)
        {
            StopWallRun();
        }
    }
}
