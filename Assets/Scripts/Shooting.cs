using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class shooting : MonoBehaviour
{
    //gun references
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] GameObject weezGun;
    [SerializeField] GameObject gun2;
    [SerializeField] GameObject crossbow;
    [SerializeField] GameObject flintlock;
    [SerializeField] Transform mountPoint;

    // gun stats
    public int damage;
    public float timeBetweenShooting, reloadSpeed, range, timeBetweenShots, spreadX, spreadY, specialAbilityCooldown;
    public int magazineZise, bulletsPerShot;
    public bool automaticFire;
    int magazineRemainingAmmo, bulletsShot;
    bool firing, readyToFire, reloading;
    bool specialAbilityReady, specialAbilityActive;

    public Transform gun;
    public Camera cam;
    public RaycastHit rayHit;
    public Transform shootingPoint;
    public LayerMask enemy;

    public GameObject muzzleFlash, bulletHole;
    [SerializeField] private TrailRenderer bulletTracer;
    private void Awake()
    {
        //on Awake it will reload ur gun so when you start its always filled with bullets. also ready to fire is set to true
        magazineRemainingAmmo = magazineZise;
        readyToFire = true;
        specialAbilityReady = true;
        
    }
    private void Update()
    {
        //every frame checking for input
        GetInput();
    }
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.G)) scoreManager.IncreaseMult(1);
        if (automaticFire)
        {
            //while user holds down the key, this allows for full automatic fire
            firing = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            //this is for single fire weapons using GetKeyDown instead of GetKey
            firing = Input.GetKeyDown(KeyCode.Mouse0);
        }
        //reload input. cannot reload with a full magazine, also cant reload while reloading.
        if (Input.GetKeyDown(KeyCode.R) && magazineRemainingAmmo < magazineZise && !reloading)
        {

            Reload();
        }
        //shoot method cant shoot while shooting, reloading or if you have no ammo in your magazine
        if (readyToFire && firing && !reloading && magazineRemainingAmmo > 0)
        {
            bulletsShot = bulletsPerShot;
            Shoot();
        }
        // Special ability input
        if (Input.GetKeyDown(KeyCode.Mouse1) && specialAbilityReady)
        {
            ActivateSpecialAbility();
        }
    }
    private void Shoot()
    {
        readyToFire = false;
        //spread
        float x = Random.Range(-spreadX, spreadX);
        float y = Random.Range(-spreadY, spreadY);
        Vector3 spreadOffset = cam.transform.right * x + cam.transform.up * y;
        Vector3 direction = cam.transform.forward + spreadOffset;
        //raycast using the random range from spread as 'direction'
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, enemy))
        {
            Debug.Log(rayHit.collider.name);
            Debug.DrawLine(transform.position, rayHit.point, Color.green, 1000f);
           
            TrailRenderer trail = Instantiate(bulletTracer, shootingPoint.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit));
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        
        //takes one bullet out of remaining ammo
        magazineRemainingAmmo--;
        //bullets shot count down by one so you can make guns using burst fire or shotgun blasts
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && magazineRemainingAmmo > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        if (magazineRemainingAmmo <= 0)
        {
            Reload();
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
        Instantiate(bulletHole, hit.point + (hit.normal * 0.1f), Quaternion.FromToRotation(Vector3.up, rayHit.normal));
        //Destroy(Trail, time);
    }
    private void ActivateSpecialAbility()
    {

        //if weezer gun is active
        if (weezGun.active)
        {
            if (!specialAbilityActive)
            {
                Debug.Log("activated weez gun special");
                mountPoint.transform.Rotate(0, 0, -90);
                spreadX = 0.01f;
                spreadY = 0.2f;
                specialAbilityActive = true;
            }
            else
            {
                Debug.Log("de-activated 9 barrle gun special");
                mountPoint.transform.Rotate(0, 0, 90);
                spreadX = 0.2f;
                spreadY = 0.01f;
                specialAbilityActive = false;
            }
        }
        else if (gun2.active)
        {
            if (!specialAbilityActive)
            {
                Debug.Log("activated 9 barrle gun special");
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
        else
        {
            Debug.Log("uh oh, no ability activated!");
        }
        // Start cooldown
        specialAbilityReady = false;
        Invoke("ResetSpecialAbility", specialAbilityCooldown);
    }
    private void ApplyKickback()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        Vector3 knockbackDirection = -cam.transform.forward + Vector3.up * 0.2f; 
        float knockbackStrength = 50f;
        StartCoroutine(ApplyKnockbackOverTime(player, knockbackDirection, knockbackStrength));
    }
    private IEnumerator ApplyKnockbackOverTime(PlayerMovement player, Vector3 direction, float strength)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            while (strength > 0)
            {

                Vector3 knockbackVelocity = direction * strength;
                controller.Move(knockbackVelocity * Time.deltaTime); // Apply movement
                //elapsed += Time.deltaTime;
                strength -= Time.deltaTime * 50;
                yield return null; // Wait until the next frame
            }
        }
    }

    private void ResetSpecialAbility()
    {
        specialAbilityReady = true;
        Debug.Log("Special ability ready!");
    }
}
