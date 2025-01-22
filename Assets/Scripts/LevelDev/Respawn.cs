using UnityEngine;

public class Respawn : MonoBehaviour
{
    Vector3 spawnPos;
    public void SpawnPos(Vector3 pSpawnPos) => spawnPos = pSpawnPos; //if there is a new spawnpos this method will be called
    public void Spawn(GameObject player) => player.transform.position = spawnPos;//when player dies this method will be called and spawns
}

