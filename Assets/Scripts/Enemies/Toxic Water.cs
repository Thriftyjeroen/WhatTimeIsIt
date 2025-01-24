using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToxicWater : MonoBehaviour
{
    // Reference to the WinLoseScript component
    WinLoseScript death;

    // Initialize the 'death' variable by getting the WinLoseScript component attached to the same GameObject
    private void Start() => death = GetComponent<WinLoseScript>();

    // Trigger detection: If an object stays inside the trigger collider, call the DeathCheck method
    private void OnTriggerStay(Collider other) => DeathCheck(other.gameObject);

    // Collision detection: If an object stays in collision with this object, call the DeathCheck method
    private void OnCollisionStay(Collision collision) => DeathCheck(collision.gameObject);

    // Method to determine what to do when an object interacts with the toxic water
    void DeathCheck(GameObject obj)
    {
        // Use a switch statement to check the name of the object that interacted
        switch (obj.gameObject.name.ToLower()) // Convert the object's name to lowercase for case-insensitive checks
        {
            // If the object's name contains "player", it is assumed to be the player, and they lose the game
            case string player when player.Contains("player"):
                death.Lose(); // Call the Lose() method from WinLoseScript to handle player death
                break;

            // If the object's name contains "enemy" or "enemie", it is assumed to be an enemy
            case string enemy when enemy.Contains("enemy"):
            case string enemies when enemies.Contains("enemie"):
                // Find an instance of the SpawnEnemies class and handle enemy despawning
                SpawnEnemies despawn = FindFirstObjectByType<SpawnEnemies>();
                despawn.EnemyDead(obj); // Call EnemyDead() to handle the enemy's death
                break;

            // For any other object, log a custom error message and destroy the object
            default:
                Debug.LogException(new Exception($"Unknown object entered the water and has been neutralized: {obj.name}"));
                Destroy(obj); // Destroy the unidentified object
                break;
        }
    }
}
