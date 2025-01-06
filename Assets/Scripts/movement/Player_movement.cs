using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    bool isGrounded;
    float speed = 5f;
    float jumpForce = 7.5f;
    float airMultiplier = 0.5f;
    int jumpCount = 0;
    int maxJumps = 2;
    Rigidbody body;
    Vector3 moveDirection;
    string layername = "ground";

    [SerializeField]float groundDrag = 4f;
    [SerializeField]float airDrag = 1f;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                jumpCount = 1;
            }
            else if (jumpCount < maxJumps)
            {
                body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }

        SpeedControl();
        AdjustDrag();
    }

    void FixedUpdate()
    {
        MovePlayer();
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
        int layerIndex = LayerMask.NameToLayer(layername);

        if (collision.gameObject.layer == layerIndex)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        int layerIndex = LayerMask.NameToLayer(layername);

        if (collision.gameObject.layer == layerIndex)
        {
            isGrounded = false;
        }
    }
}
