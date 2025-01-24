using UnityEngine;
using UnityEngine.SceneManagement;

public class Bomb : MonoBehaviour
{
    // Explosion parameters
    private float explosionForce = 500;
    private float explosionRadius = 10;
    private float upwardsModifier = 10;

    // Timer settings for explosion
    private float maxTime = 4;
    private float currentTime = 0;
    [SerializeField] float gravityScale;
    private float maxDamage = 100;
    // Explosion effects
    [SerializeField] GameObject explosionSound;
    [SerializeField] GameObject particles;

    [SerializeField] private ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the bomb should explode due to timeout
        currentTime += Time.deltaTime;
        if(currentTime > maxTime)
        {
            Explode(0);
        }
        // Apply gravity to the bombs rigidbody
        GetComponent<Rigidbody>().linearVelocity -= Vector3.up * gravityScale;
    }

    public void Explode(int _shot)
    {
        // Instantiate visual and sound effects for the explosion
        Instantiate(particles, transform.position, transform.rotation);
        Instantiate(explosionSound);
        // Create a list of colliders within the explosion radius
        Collider[] hitColliders = new Collider[10];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders);

        int enemiesHit = 0;
        // Iterate through all colliders hit by the explosion
        for (int i = 0; i < collidersHit; i++)
        {
            // Apply explosion force to any Rigidbody components
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, upwardsModifier);
            }
            // Apply damage to enemies within the explosion radius
            if (hitColliders[i].TryGetComponent(out EnemyHealth enemy))
            {
                enemiesHit++;
                // Calculate damage based on distance from the explosion center
                float damage = maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * maxDamage / explosionRadius;
                if (damage > 0)
                {
                    // Deal damage to the enemy and increase the score
                    enemy.TakeDamage((int)Mathf.Round(damage));
                    scoreManager.IncreaseScore((int)Mathf.Round(damage));
                    // If the bomb was shot, increase the score multiplier
                    if (_shot > 0)
                    {
                        scoreManager.IncreaseMult(_shot);
                        _shot--;
                    }
                    

                }
                
            }
            
        }


        // Destroy the bomb object after the explosion
        Destroy(gameObject);
    }
}
