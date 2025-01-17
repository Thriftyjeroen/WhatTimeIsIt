using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToxicWater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name.ToLower()) 
        {
            case string player when player.Contains("player"):
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case string enemy when enemy.Contains("enemy"):
            case string enemies when enemies.Contains("enemie"):
                SpawnEnemies despawn = FindObjectOfType<SpawnEnemies>();
                despawn.EnemyDead(other.gameObject);
                break;
            default:
                Debug.LogException(new Exception($"Unknown object enterd the water and is nutrelized: {other.gameObject.name}"));//custom error message
                Destroy(other.gameObject);
                //throw new Exception("test");
                break;
        }
    }
}
