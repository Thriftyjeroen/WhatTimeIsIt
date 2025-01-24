using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] Transform target; // Reference to the target the enemy will follow (player)
    private NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding
    bool resetPos = false; // Flag to check if the position should be reset

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component on the enemy
    }

    void Update()
    {
        float detectionRadius = 20f; // Set detection radius for the enemy

        // Get all colliders within the sphere radius around the enemy's position
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        if (target != null)
        {
            bool playerFound = false;

            // Loop through each collider detected within the radius
            foreach (Collider col in hitColliders)
            {
                // Check if the collider has the "Player" tag, indicating the player is nearby
                if (col.CompareTag("Player"))
                {
                    playerFound = true; // Set playerFound to true if the player is detected
                }
            }

            if (!playerFound)
            {
                // If the player is not found, continue moving towards the target
                if (resetPos)
                {
                    // If resetPos is true, the enemy returns to its last position
                    agent.nextPosition = transform.position;
                    resetPos = false;
                }
                agent.isStopped = false; // Ensure the enemy keeps moving
                agent.updatePosition = true; // Allow position updates
                agent.SetDestination(target.position); // Set the target destination for the enemy
            }
            else
            {
                // If the player is found, stop updating position and stop movement
                resetPos = true;
                agent.updatePosition = false; // Stop position updates as the enemy is at the target
                agent.SetDestination(target.position); // Set the target destination to the player
            }
        }
    }
}
