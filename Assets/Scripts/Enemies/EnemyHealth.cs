using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int currentHealth;

    private SpawnEnemies spawnEnemies;
    private EnemyTypeSettings enemyTypeSettings;

    void Start()
    {
        // Get the EnemyTypeSettings component and initialize health
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

        spawnEnemies = FindFirstObjectByType<SpawnEnemies>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (spawnEnemies != null)
        {
            spawnEnemies.EnemyDead(this.gameObject);
        }

        Destroy(gameObject);
    }
}
