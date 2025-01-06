using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    bool activated = false;
    [SerializeField] float respawnTimer;
    [SerializeField] int amaunt;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<Vector3> spawnPos = new List<Vector3>();
    float time = 0;


    void Start() => SpawnFirstWave();
    void Update() => Reinforcement();
    void SpawnFirstWave()//spawns the starting enamies
    {
        for (int i = 0; i < amaunt; i++) Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos[Random.Range(0, spawnPos.Count)], Quaternion.identity);
    }
    void Reinforcement()//if the player unlockes the area there will be at least 1 new enemie spawned based on the number of the respawnTimer
    {
        if (!activated) return;
        time += Time.deltaTime;
        float rNumber = Random.Range(time,respawnTimer);
        if (rNumber >= respawnTimer) Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos[Random.Range(0, spawnPos.Count)],Quaternion.identity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") activated = true;
    }
}
