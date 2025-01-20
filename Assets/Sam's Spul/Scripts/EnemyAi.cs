using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] Transform target; 
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float detectionRadius = 20f;

        // Get all colliders within the sphere radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);


        if (target != null)
        {
            bool playerFound = false;
            // Loop through each collider in the hitColliders array
            foreach (Collider col in hitColliders)
            {
                // Check if the collider has the "Player" tag
                if (col.CompareTag("Player"))
                {
                    playerFound = true;
                }
            }

            if (!playerFound)
            {
                agent.enabled = true;
                agent.SetDestination(target.position);
            }
            else
            {
                agent.enabled = false;
            }
        }
    }
}
