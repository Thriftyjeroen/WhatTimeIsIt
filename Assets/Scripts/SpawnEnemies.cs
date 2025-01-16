using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] int amaunt;
    [SerializeField] float respawnTimer;
    [SerializeField] GameObject parrond;

    [SerializeField] List<GameObject> spawnPos = new List<GameObject>();
    [SerializeField] List<GameObject> enemyTypes = new List<GameObject>();
    [SerializeField] List<GameObject> enemiesNotActive = new List<GameObject>();
    List<GameObject> enemiesActive = new List<GameObject>();

    bool activated = false;
    float time = 0;

    void Start() => SpawnFirstWave();
    void Update() => Reinforcement(); 
    void SpawnFirstWave()//spawns the starting enemies
    { 
        while(enemiesNotActive.Count != 0)
        {
            GameObject enemy = enemiesNotActive[0];
            enemy.SetActive(true);
            enemiesActive.Add(enemy);
            enemiesNotActive.Remove(enemy);
        }
    }
    void Reinforcement()//if the player unlockes the area there will be at least 1 new enemy spawned based on the number of the respawnTimer
    {
        if (!activated) return;
        time += Time.deltaTime;
        if (time < respawnTimer) return;
        if (enemiesNotActive.Count != 0)
        {
            GameObject reïnforcement = enemiesNotActive[Random.Range(0, enemiesNotActive.Count)];
            reïnforcement.SetActive(true);
            reïnforcement.transform.position = spawnPos[Random.Range(0, spawnPos.Count)].transform.position;
            enemiesActive.Add(reïnforcement);
            enemiesNotActive.Remove(reïnforcement);
        }
        else enemiesActive.Add(Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)],parrond.transform));
        time -= 10;
    }
    private void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name.ToLower();
        if (name.Contains("player")) activated = true;
    }
    public void EnemyDead(GameObject enemy)
    {
        enemiesNotActive.Add(enemy);
        enemiesActive.Remove(enemy);
        enemy.SetActive(false);
    }
}