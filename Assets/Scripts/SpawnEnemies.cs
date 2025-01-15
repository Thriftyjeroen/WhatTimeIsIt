using System.Collections.Generic;
using UnityEngine;

public class Spawnenemies : MonoBehaviour
{
    [SerializeField] int amaunt;
    [SerializeField] float respawnTimer;
    [SerializeField] GameObject parrond;

    [SerializeField] List<GameObject> spawnPos = new List<GameObject>();
    [SerializeField] List<GameObject> enemyTypes = new List<GameObject>();
    [SerializeField] List<GameObject> enemiesNotActive = new List<GameObject>();
    [SerializeField] List<GameObject> enemiesActive = new List<GameObject>();

    bool activated = false;
    float time = 0;


    void Start() => SpawnFirstWave();
    void Update() => Reinforcement();
    void SpawnFirstWave()//spawns the starting enemies
    {
        //for (int i = 0; i < amaunt; i++) Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos[Random.Range(0, spawnPos.Count)].transform.position, Quaternion.identity);
        foreach (GameObject enemy in enemiesNotActive) enemy.SetActive(true);
    }
    void Reinforcement()//if the player unlockes the area there will be at least 1 new enemy spawned based on the number of the respawnTimer
    {
        if (!activated) return;
        time += Time.deltaTime;
        print($"{time},{respawnTimer}");
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
        print(activated + " " + name);
    }
    public void enemyDead(GameObject enemy)
    {
        enemiesNotActive.Add(enemy);
        enemiesActive.Remove(enemy);
        enemy.SetActive(false);
    }
}
