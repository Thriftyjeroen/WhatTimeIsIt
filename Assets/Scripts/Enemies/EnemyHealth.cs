using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;  // The current health of the enemy

    private SpawnEnemies spawnEnemies;  // Reference to the SpawnEnemies component
    private EnemyTypeSettings enemyTypeSettings;  // Reference to the EnemyTypeSettings component

    void Start()
    {
        // Get the EnemyTypeSettings component and initialize health
        enemyTypeSettings = GetComponent<EnemyTypeSettings>();
        if (enemyTypeSettings != null)
        {
            enemyTypeSettings.InitializeHealth();  // Set the initial health based on enemy type
            currentHealth = enemyTypeSettings.maxHealth;  // Set current health to max health
        }
        else
        {
            Debug.LogWarning("EnemyTypeSettings component is missing!");  // Warning if no EnemyTypeSettings component is found
        }

        // Find the SpawnEnemies component
        spawnEnemies = FindFirstObjectByType<SpawnEnemies>();
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Reduce health by damage amount

        // Check if health is zero or below, and trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method called when the enemy dies
    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");  // Log death message

        // Notify SpawnEnemies if the component is available
        if (spawnEnemies != null)
        {
            spawnEnemies.EnemyDead(this.gameObject);  // Call EnemyDead on the SpawnEnemies component
        }

        Destroy(gameObject);  // Destroy the enemy object
    }
}
