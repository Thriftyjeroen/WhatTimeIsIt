using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRange = 10f; // Range at which the enemy detects the player
    public float attackRange = 2f; // Range at which the enemy attacks the player
    public float wanderRadius = 5f; // Radius for random wandering
    public float wanderTime = 3f; // Time between direction changes when wandering
    public float moveSpeed = 3f; // Movement speed of the enemy
    public LayerMask obstacleLayer; // Layer mask for detecting obstacles

    private Vector3 wanderTarget; // Current target position for wandering
    private float wanderTimer; // Timer to track wandering interval

    void Start()
    {
        wanderTarget = transform.position; // Initialize wander target at current position
        wanderTimer = wanderTime; // Initialize the timer
    }

    void Update()
    {
        transform.forward = wanderTarget; // Ensure enemy faces the target direction

        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate distance to player

        if (distanceToPlayer <= detectionRange)
        {
            // Move towards the player if within detection range
            MoveTowards(player.position);
        }
        else
        {
            // Handle random wandering behavior
            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderTime)
            {
                // Get a new random position when timer exceeds the wander interval
                wanderTarget = GetRandomPosition(transform.position, wanderRadius);
                wanderTimer = 0; // Reset the timer
            }

            // Move towards the current wander target
            MoveTowards(wanderTarget);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized; // Calculate direction to the target
        RaycastHit hit;

        // Check if an obstacle is in the way
        if (!Physics.Raycast(transform.position, direction, out hit, 1f, obstacleLayer))
        {
            // Move towards the target if no obstacle is detected
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    Vector3 GetRandomPosition(Vector3 origin, float radius)
    {
        // Generate a random position within the specified radius
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        randomDirection.y = origin.y; // Maintain the same height
        return randomDirection;
    }
}
