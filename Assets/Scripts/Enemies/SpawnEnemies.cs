using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    //[SerializeField] int ammount; // Number of enemies (not used)
    [SerializeField] float respawnTimer; // Time between enemy spawns
    [SerializeField] GameObject parrond; // Parent object for spawned enemies

    [SerializeField] List<GameObject> spawnPos = new List<GameObject>(); // Spawn positions for enemies
    [SerializeField] List<GameObject> enemyTypes = new List<GameObject>(); // Enemy types to spawn
    [SerializeField] List<GameObject> enemiesNotActive = new List<GameObject>(); // Inactive enemies ready to spawn
    [SerializeField] List<GameObject> enemiesActive = new List<GameObject>(); // Active enemies in the game

    bool activated = false; // Checks if spawning is activated
    float time = 0; // Timer for respawn interval

    void Start() => SpawnFirstWave(); // Spawn the first set of enemies
    void Update() => Reinforcement(); // Spawn more enemies over time

    // Spawns the first wave of enemies
    void SpawnFirstWave()
    {
        // Activate all inactive enemies and add them to the active list
        while (enemiesNotActive.Count != 0)

        {
            GameObject enemy = enemiesNotActive[0];
            enemy.SetActive(true);
            enemiesActive.Add(enemy);
            enemiesNotActive.Remove(enemy);
        }
        activated = true; // Mark spawning as activated
    }

    // Spawns reinforcement enemies after a certain time
    void Reinforcement()
    {
        if (!activated) return; // Do nothing if spawning is not activated
        time += Time.deltaTime; // Increase timer
        if (time <= respawnTimer) return; // Wait until enough time has passed

        GameObject reïnforcement;

        // Spawn an inactive enemy if available
        if (enemiesNotActive.Count != 0)

        {
            reïnforcement = enemiesNotActive[Random.Range(0, enemiesNotActive.Count)];
            reïnforcement.SetActive(true);
            enemiesNotActive.Remove(reïnforcement);
        }
        // If no inactive enemies, create a new one from the enemy types
        else reïnforcement = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], parrond.transform);

        reïnforcement.transform.position = spawnPos[Random.Range(0, spawnPos.Count)].transform.position;//the postition of the new enemy will be set to the position of a random spawnpoint for the enemies
        enemiesActive.Add(reïnforcement); // Add to active enemies
        time = 0; // Reset timer
    }

    // Detect when the player enters the area and activates spawning
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().Contains("player")) activated = true;
    }
    // Handle when an enemy dies and is returned to the inactive list
    public void EnemyDead(GameObject enemy)
    {
        enemiesNotActive.Add(enemy); // Add to inactive list
        enemiesActive.Remove(enemy); // Remove from active list
        enemy.SetActive(false); // Deactivate enemy
    }
}