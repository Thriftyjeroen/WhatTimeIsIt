using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject crossbow;
    [SerializeField] Transform mountPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Animator animatorCrossbow;

    float lastShot;
    [SerializeField] Transform bulletSpawnPoint;
    GameObject player;

    public int damage;
    public float timeBetweenShooting, reloadSpeed, range, timeBetweenShots, spreadX, spreadY, specialAbilityCooldown;
    public int magazineZise, bulletsPerShot;
    public bool automaticFire;
    int magazineRemainingAmmo, bulletsShot;
    bool firing, readyToFire, reloading;
    bool specialAbilityReady, specialAbilityActive;

    public RaycastHit rayHit;
    public Transform shootingPoint;

    private void Awake()
    {
        //on Awake it will reload ur gun so when you start its always filled with bullets. also ready to fire is set to true
        magazineRemainingAmmo = magazineZise;
        readyToFire = true;
        specialAbilityReady = true;
        
    }
    private void Update()
    {
        FindPlayer();
        Shoot();
    }

    public void FindPlayer()
    {
        // Set the detection radius
        float detectionRadius = 10f;

        // Get all colliders within the sphere radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);


        // Loop through each collider in the hitColliders array
        foreach (Collider col in hitColliders)
        {
            // Check if the collider has the "Player" tag
            if (col.CompareTag("Player"))
            {
                player = col.gameObject;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        // Check if the closest enemy is within shooting range and cooldown has passed
        if (player != null 
        && Time.time > lastShot + 3)
        {
            animatorCrossbow.SetTrigger("Reload");

            // Get a bullet from the object pool
            GameObject bullet = Instantiate(bulletPrefab);
            if (bullet != null)
            {
                // Set bullet's position and rotation
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);


                // Update the last shoot time
                lastShot = Time.time;
            }
        }
    }
    private void ResetShot()
    {
        //reset shot is used with Invoke so different guns can have different firing speeds. 
        readyToFire = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadSpeed);
    }
    private void ReloadFinished()
    {
        //reload finished used with Invoke so different guns can have different reloading speeds
        magazineRemainingAmmo = magazineZise;
        reloading = false;
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        while (time < 1)
        {
            //t.transform.position += t.transform.forward * 10f * Time.deltaTime;
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = hit.point;
        //Destroy(Trail, time);
    }
}
