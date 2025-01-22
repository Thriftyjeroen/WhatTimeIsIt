using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] Transform mountPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Animator animator;
    [SerializeField] Transform bulletSpawnPoint;
    GameObject player;

    public int damage;
    public float timeBetweenShooting, reloadSpeed, timeBetweenShots;
    public int magazineZise, bulletsPerShot;
    public bool automaticFire;
    int magazineRemainingAmmo, bulletsShot;
    bool readyToFire, reloading;

    public RaycastHit rayHit;
    public Transform shootingPoint;

    private void Awake()
    {
        //on Awake it will reload ur gun so when you start its always filled with bullets. also ready to fire is set to true
        magazineRemainingAmmo = magazineZise;
        readyToFire = true;

    }
    private void Update()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        // Set the detection radius
        float detectionRadius = 20f;

        // Get all colliders within the sphere radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        // Loop through each collider in the hitColliders array
        foreach (Collider col in hitColliders)
        {
            // Check if the collider has the "Player" tag
            if (col.CompareTag("Player"))
            {
                player = col.gameObject;
                EnemyShoot();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 20f);
    }

    private void EnemyShoot()
    {
        Debug.Log("EnemyShooting.cs shoot() triggered");
        //shoot method cant shoot while shooting, reloading or if you have no ammo in your magazine
        if (readyToFire && !reloading && magazineRemainingAmmo > 0)
        {

            readyToFire = false;
            //raycast using the random range from spread as 'direction'
            animator.SetTrigger("Reload");
            // Get a bullet from the object pool
            GameObject bullet = Instantiate(bulletPrefab);
            if (bullet != null)
            {
                // Set bullet's position and rotation
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            }
            //takes one bullet out of remaining ammo
            magazineRemainingAmmo--;
            //bullets shot count down by one so you can make guns using burst fire or shotgun blasts
            bulletsShot--;
            Invoke("ResetShot", timeBetweenShooting);
            if (bulletsShot > 0 && magazineRemainingAmmo > 0)
            {
                Invoke("EnemyShoot", timeBetweenShots);
            }
            if (magazineRemainingAmmo <= 0)
            {
                Reload();
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
}
