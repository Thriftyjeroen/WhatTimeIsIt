using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Enemy's starting health
    public int maxHealth = 100;
    private int currentHealth;
   [SerializeField] shooting shooting;
   SpawnEnemies spawnEnemies; 

    // Reference to a possible death effect
  //  public GameObject deathEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the enemy's health to the maximum health
        currentHealth = maxHealth;
      spawnEnemies = FindFirstObjectByType<SpawnEnemies>();
    }

    // Update is called once per frame
    void Update()
    {
        // Placeholder for player attack logic
        // Example: Replace with actual attack detection logic (e.g., collision or raycast)
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Simulate attack with space key
        {
            // Check weapon type and deal damage
            TakeDamage(shooting.damage);
        }
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle the enemy's death
    private void Die()
    {
        Debug.Log("Enemy died!");

        // Play death effect if assigned
        // if (deathEffect != 0)
        // {
        //     Instantiate(deathEffect, transform.position, Quaternion.identity);
        // }

        // Destroy the enemy GameObject
        spawnEnemies.EnemyDead(this.gameObject);

    }
}
