using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;

    private SpawnEnemies spawnEnemies;
    private EnemyTypeSettings enemyTypeSettings;

    void Start()
    {
        // Get the enemy type settings and set health
        enemyTypeSettings = GetComponent<EnemyTypeSettings>();
        if (enemyTypeSettings != null)
        {
            enemyTypeSettings.InitializeHealth();
            currentHealth = enemyTypeSettings.maxHealth;
        }
        else
        {
            Debug.LogWarning("EnemyTypeSettings component is missing!");
        }

        // Find the SpawnEnemies component
        spawnEnemies = FindFirstObjectByType<SpawnEnemies>();
    }

    // Reduce health when the enemy takes damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die(); // Kill the enemy if health is zero or less
        }
    }

    // Handle enemy death
    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (spawnEnemies != null)
        {
            spawnEnemies.EnemyDead(this.gameObject); // Notify spawn manager
        }

        Destroy(gameObject); // Destroy the enemy
    }
}
