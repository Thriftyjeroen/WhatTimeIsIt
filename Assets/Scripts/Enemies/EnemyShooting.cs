using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject weapon;  // The weapon the enemy is using
    [SerializeField] Transform mountPoint;  // The point where the weapon is mounted
    [SerializeField] GameObject bulletPrefab;  // Bullet prefab for the weapon
    [SerializeField] Animator animator;  // Animator for the enemy's shooting animation
    [SerializeField] Transform bulletSpawnPoint;  // The spawn point for the bullets
    GameObject player;  // Reference to the player

    public int damage;  // Damage the enemy does with each shot
    public float timeBetweenShooting, reloadSpeed, timeBetweenShots;  // Timings for shooting and reloading
    public int magazineZise, bulletsPerShot;  // Magazine size and number of bullets fired per shot
    public bool automaticFire;  // If true, the enemy shoots automatically
    int magazineRemainingAmmo, bulletsShot;  // Ammo count and shot count
    bool readyToFire, reloading;  // Flags for shooting and reloading status

    public RaycastHit rayHit;  // Raycast hit information
    public Transform shootingPoint;  // The point from where the ray will be cast

    private void Awake()
    {
        // Initialize ammo and firing readiness at the start
        magazineRemainingAmmo = magazineZise;
        readyToFire = true;
    }

    private void Update()
    {
        FindPlayer();  // Look for the player every frame
    }

    private void FindPlayer()
    {
        // Set the detection radius for the enemy
        float detectionRadius = 20f;

        // Get all colliders within the detection radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        // Loop through all detected colliders
        foreach (Collider col in hitColliders)
        {
            // If the collider has the "Player" tag, start shooting
            if (col.CompareTag("Player"))
            {
                player = col.gameObject;
                EnemyShoot();  // Call the shooting method
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection radius in the editor
        Gizmos.DrawWireSphere(transform.position, 20f);
    }

    private void EnemyShoot()
    {
        Debug.Log("EnemyShooting.cs shoot() triggered");

        // Check if the enemy is ready to fire
        if (readyToFire && !reloading && magazineRemainingAmmo > 0)
        {
            readyToFire = false;
            animator.SetTrigger("Reload");  // Trigger reload animation (may be used for visual effects)

            // Instantiate a bullet and set its position and direction
            GameObject bullet = Instantiate(bulletPrefab);
            if (bullet != null)
            {
                bullet.transform.position = bulletSpawnPoint.position;  // Bullet spawn position
                bullet.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);  // Bullet direction
            }

            // Deduct one bullet from the magazine
            magazineRemainingAmmo--;
            bulletsShot--;  // Decrease the number of bullets to be fired in case of burst shots

            // Reset shooting state after the shooting delay
            Invoke("ResetShot", timeBetweenShooting);

            // Fire again if there are bullets remaining in the magazine
            if (bulletsShot > 0 && magazineRemainingAmmo > 0)
            {
                Invoke("EnemyShoot", timeBetweenShots);  // Shoot again after the delay
            }

            // Reload if magazine is empty
            if (magazineRemainingAmmo <= 0)
            {
                Reload();
            }
        }
    }

    private void ResetShot()
    {
        // Reset the ready to fire state to allow shooting again
        readyToFire = true;
    }

    private void Reload()
    {
        reloading = true;  // Set the reloading flag to true
        Invoke("ReloadFinished", reloadSpeed);  // Call ReloadFinished after the reload time
    }

    private void ReloadFinished()
    {
        // Reset ammo and stop reloading
        magazineRemainingAmmo = magazineZise;
        reloading = false;
    }
}
