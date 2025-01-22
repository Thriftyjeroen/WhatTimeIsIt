using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToxicWater : MonoBehaviour
{
    WinLoseScript death;
    private void Start() => death = GetComponent<WinLoseScript>();
    private void OnTriggerStay(Collider other) => DeathCheck(other.gameObject);
    private void OnCollisionStay(Collision collision) => DeathCheck(collision.gameObject);
    void DeathCheck(GameObject obj)
    {
        //print("test");
        switch (obj.gameObject.name.ToLower())
        {
            case string player when player.Contains("player"):
                death.Lose();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //print(SceneManager.GetActiveScene().name);
                break;
            case string enemy when enemy.Contains("enemy"):
            case string enemies when enemies.Contains("enemie"):
                SpawnEnemies despawn = FindObjectOfType<SpawnEnemies>();
                despawn.EnemyDead(obj);
                break;
            default:
                Debug.LogException(new Exception($"Unknown object enterd the water and has been neutralized: {obj.name}"));//custom error message
                Destroy(obj);
                //throw new Exception("test"); 
                break;
        }
    }
}
