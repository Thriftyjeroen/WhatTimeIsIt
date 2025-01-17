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

    private static GameObject particles;
    [SerializeField] private ScoreManager scoreManager;
    void Start()
    {
        if (!particles)
        {
            particles = Resources.Load<GameObject>("Particle System");
        }
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
        Collider[] hitColliders = new Collider[10];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders);
        for(int i = 0; i < collidersHit; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, upwardsModifier);
            }
            if (hitColliders[i].TryGetComponent(out EnemyHealth enemy))
            {
                float damage = maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * maxDamage / explosionRadius;
                if (damage > 0)
                {
                    enemy.TakeDamage((int)Mathf.Round(damage));
                    scoreManager.IncreaseScore((int)Mathf.Round(damage));
                    scoreManager.IncreaseMult(_shot);
                }
                
            }
            
        }
        
        //Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
