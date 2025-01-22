using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    float startTime = 0;
    Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * speed;

       if (Time.time - startTime > 10) Destroy(gameObject); // Skibidi
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamager(10);
        }
        Destroy(gameObject);
    }

}
