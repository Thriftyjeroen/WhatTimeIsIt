using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Referentie naar de speler
    public float detectionRange = 10f; // Afstand waarop de vijand de speler kan zien
    public float attackRange = 2f; // Afstand waarop de vijand aanvalt
    public float wanderRadius = 5f; // Radius voor willekeurig rondlopen
    public float wanderTime = 3f; // Tijd tussen het veranderen van richtingen
    public float moveSpeed = 3f; // Snelheid van de vijand
    public LayerMask obstacleLayer; // Obstakels om rekening mee te houden

    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        wanderTarget = transform.position; // Start op huidige positie
        wanderTimer = wanderTime;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Naar de speler bewegen
            MoveTowards(player.position);

            if (distanceToPlayer <= attackRange)
            {
                // Aanval uitvoeren (placeholder)
                Debug.Log("Aanvallen!");
            }
        }
        else
        {
            // Willekeurig rondlopen
            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderTime)
            {
                wanderTarget = GetRandomPosition(transform.position, wanderRadius);
                wanderTimer = 0;
            }

            MoveTowards(wanderTarget);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        RaycastHit hit;

        // Controleer of er een obstakel in de weg is
        if (!Physics.Raycast(transform.position, direction, out hit, 1f, obstacleLayer))
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    Vector3 GetRandomPosition(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        randomDirection.y = origin.y; // Houd dezelfde hoogte aan
        return randomDirection;
    }
}
