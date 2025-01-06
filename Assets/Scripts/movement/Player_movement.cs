using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumppower = 16.5f;
    bool grounded;
    string layername = "ground";
    [SerializeField] Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
            grounded = false;
        }
    }
    private void Jump()
    {
        
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        int layerIndex = LayerMask.NameToLayer(layername);

        if (collision.gameObject.layer == layerIndex)
        {
            grounded = true;
        }
    }
}
