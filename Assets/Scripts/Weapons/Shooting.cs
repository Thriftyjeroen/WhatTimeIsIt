using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class shooting : MonoBehaviour
{
    // Gun references
    [SerializeField] GameObject weezGun; // Reference to the 'weezer' gun
    [SerializeField] GameObject gun2; // Reference to the second gun
    [SerializeField] GameObject crossbow; // Reference to the crossbow
    [SerializeField] GameObject flintlock; // Reference to the flintlock
    [SerializeField] Transform mountPoint; // The mount point for the gun

    // Gun stats
    public int damage; // The damage dealt by the weapon
    public float timeBetweenShooting, reloadSpeed, range, timeBetweenShots, spreadX, spreadY, specialAbilityCooldown; // Various parameters related to shooting mechanics
    public int magazineZise, bulletsPerShot; // Magazine size and bullets per shot
    public bool automaticFire; // Whether the gun fires automatically or not
    int magazineRemainingAmmo, bulletsShot; // Track the remaining ammo in the magazine and the number of bullets shot
    bool firing, readyToFire, reloading; // Flags for managing firing, readiness to fire, and reloading
    bool specialAbilityReady, specialAbilityActive; // Flags for special ability activation and readiness

    // Raycast and other references for shooting
    public Transform gun; // The gun transform
    public Camera cam; // The camera to determine the shooting direction
    public RaycastHit rayHit; // Raycast hit details
    public Transform shootingPoint; // The point from where the bullet is fired
    public LayerMask enemy, player; // Layers for raycast detection
    [SerializeField] private GameObject bomb; // Reference to the bomb object
    [SerializeField] private float bombThrowForce; // Force applied when throwing the bomb
    public GameObject bulletHole; // Prefab for bullet hole effect
    [SerializeField] private TrailRenderer bulletTracer; // Bullet tracer effect
    [SerializeField] private ScoreManager scoreManager; // Reference to the score manager

    private void Awake()
    {
        // Initializes the gun with a full magazine on start and sets the weapon to be ready to fire
        magazineRemainingAmmo = magazineZise;
        readyToFire = true;
        specialAbilityReady = true;
    }

    private void Update()
    {
        // Every frame, check for input related to firing and special abilities
        GetInput();
    }

    private void GetInput()
    {
        if (automaticFire)
        {
            // If automatic fire is enabled, fire while holding down the mouse button
            firing = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            // For single-shot weapons, fire when the mouse button is pressed
            firing = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Reload input: can't reload if the magazine is full or if already reloading
        if (Input.GetKeyDown(KeyCode.R) && magazineRemainingAmmo < magazineZise && !reloading)
        {
            Reload();
        }

        // Shoot input: can shoot if ready to fire, not reloading, and there is ammo
        if (readyToFire && firing && !reloading && magazineRemainingAmmo > 0)
        {
            bulletsShot = bulletsPerShot;
            Shoot();
        }

        // Special ability input: activate the special ability if ready
        if (Input.GetKeyDown(KeyCode.Mouse1) && specialAbilityReady)
        {
            ActivateSpecialAbility();
        }
    }

    private void Shoot()
    {
        readyToFire = false;

        // Apply spread to simulate gun recoil/spread
        float x = Random.Range(-spreadX, spreadX);
        float y = Random.Range(-spreadY, spreadY);
        Vector3 spreadOffset = cam.transform.right * x + cam.transform.up * y;
        Vector3 direction = cam.transform.forward + spreadOffset;

        // Perform raycast to detect what the bullet hits
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, ~player))
        {
            Debug.DrawLine(transform.position, rayHit.point, Color.green, 1000f);

            // Bullet hole effect at the impact point
            Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.1f), Quaternion.FromToRotation(Vector3.up, rayHit.normal));

            // Bullet tracer effect
            TrailRenderer trail = Instantiate(bulletTracer, shootingPoint.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit));

            // If an enemy is hit, apply damage and update the score
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                scoreManager.IncreaseScore(damage);
            }

            // If a bomb is hit, trigger the explosion
            if (rayHit.collider.CompareTag("Bomb"))
            {
                rayHit.collider.GetComponent<Bomb>().Explode(1);
            }
        }

        // Decrease ammo count after shooting
        magazineRemainingAmmo--;
        bulletsShot--;

        // Reset shot readiness after a delay based on firing rate
        Invoke("ResetShot", timeBetweenShooting);

        // If there are more bullets to shoot, fire again after the timeBetweenShots delay
        if (bulletsShot > 0 && magazineRemainingAmmo > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        // Reload if the magazine is empty
        if (magazineRemainingAmmo <= 0)
        {
            Reload();
        }
    }

    private void ResetShot()
    {
        // Reset shot readiness to true after a delay
        readyToFire = true;
    }

    private void Reload()
    {
        // Start the reloading process
        reloading = true;
        Invoke("ReloadFinished", reloadSpeed);
    }

    private void ReloadFinished()
    {
        // Finish reloading and refill the magazine
        magazineRemainingAmmo = magazineZise;
        reloading = false;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        while (time < 1)
        {
            // Move the trail to the hit point over time
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = hit.point;
    }

    private void ActivateSpecialAbility()
    {
        // Handle special abilities for different guns
        if (weezGun.activeInHierarchy)
        {
            // If the 'weezer' gun is active, toggle the special ability
            if (!specialAbilityActive)
            {
                mountPoint.transform.Rotate(0, 0, -90); // Rotate the mount point
                spreadX = 0.01f;
                spreadY = 0.2f;
                specialAbilityActive = true;
            }
            else
            {
                mountPoint.transform.Rotate(0, 0, 90);
                spreadX = 0.2f;
                spreadY = 0.01f;
                specialAbilityActive = false;
            }
        }
        else if (gun2.activeInHierarchy)
        {
            // Special ability for the second gun: rapid fire
            if (specialAbilityReady)
            {
                specialAbilityReady = false;
                spreadX = 0.1f;
                spreadY = 0.1f;
                timeBetweenShots = 0.01f;
                bulletsShot = bulletsPerShot;
                Shoot();
                firing = true;
                ApplyKickback();
                if (!firing)
                {
                    timeBetweenShots = 0.08f;
                }
            }
        }
        else if (flintlock.activeInHierarchy)
        {
            // Special ability for the flintlock: throw a bomb
            if (specialAbilityReady)
            {
                specialAbilityReady = false;
                GameObject obj = Instantiate(bomb, transform.position + transform.forward * 1, Quaternion.identity);
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.AddForce((-transform.forward + Vector3.up) * bombThrowForce, ForceMode.Impulse);
            }
        }
        else if (crossbow.activeInHierarchy)
        {
            // Special ability for the crossbow: knockback enemies
            if (specialAbilityReady)
            {
                specialAbilityReady = false;
                RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, range, enemy);
                foreach (RaycastHit hit in hits)
                {
                    GameObject enemy = hit.collider.gameObject;
                    if (enemy.TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddForce((-transform.forward) * 10f, ForceMode.Impulse);
                    }
                    if (enemy.TryGetComponent(out EnemyHealth hp))
                    {
                        hp.TakeDamage(damage);
                        scoreManager.IncreaseScore(damage);
                        if (hits.Length > 2)
                        {
                            scoreManager.IncreaseMult(1);
                        }
                    }
                }
            }
        }

        // Start special ability cooldown
        specialAbilityReady = false;
        Invoke("ResetSpecialAbility", specialAbilityCooldown);
    }

    private void ApplyKickback()
    {
        // Apply a knockback effect to the player when firing a powerful weapon
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        Vector3 knockbackDirection = -cam.transform.forward + Vector3.up * 0.2f;
        float knockbackStrength = 50f;
        StartCoroutine(ApplyKnockbackOverTime(player, knockbackDirection, knockbackStrength));
    }

    private IEnumerator ApplyKnockbackOverTime(PlayerMovement player, Vector3 direction, float strength)
    {
        // Apply knockback over time to simulate the recoil effect
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            while (strength > 0)
            {
                Vector3 knockbackVelocity = direction * strength;
                controller.Move(knockbackVelocity * Time.deltaTime);
                strength -= Time.deltaTime * 50;
                yield return null;
            }
        }
    }

    private void ResetSpecialAbility()
    {
        // Reset special ability readiness after the cooldown
        specialAbilityReady = true;
    }
}
