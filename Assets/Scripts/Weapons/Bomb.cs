using UnityEngine;
using UnityEngine.SceneManagement;

public class Bomb : MonoBehaviour
{
    private float explosionForce = 500;
    //explosion size, radius and height
    private float explosionRadius = 10;
    private float upwardsModifier = 10;

    //how long it takes to explode
    private float maxTime = 4;
    private float currentTime = 0;
    [SerializeField] float gravityScale;
    private float maxDamage = 100;

    [SerializeField] GameObject explosionSound;
    [SerializeField] GameObject particles;

    [SerializeField] private ScoreManager scoreManager;
    void Start()
    {
        // SKib
    }
    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;
        if(currentTime > maxTime)
        {
            Explode(0);
        }
        GetComponent<Rigidbody>().linearVelocity -= Vector3.up * gravityScale;
    }

    public void Explode(int _shot)
    {
        Instantiate(particles, transform.position, transform.rotation);
        Instantiate(explosionSound);
        Collider[] hitColliders = new Collider[10];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders);

        int enemiesHit = 0;
        for(int i = 0; i < collidersHit; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, upwardsModifier);
            }
            if (hitColliders[i].TryGetComponent(out EnemyHealth enemy))
            {
                enemiesHit++;
                float damage = maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * maxDamage / explosionRadius;
                if (damage > 0)
                {
                    enemy.TakeDamage((int)Mathf.Round(damage));
                    scoreManager.IncreaseScore((int)Mathf.Round(damage));
                    if (_shot > 0)
                    {
                        scoreManager.IncreaseMult(_shot);
                        _shot--;
                    }
                    

                }
                
            }
            
        }
        


        Destroy(gameObject);
    }
}
