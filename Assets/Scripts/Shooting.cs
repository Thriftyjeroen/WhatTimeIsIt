using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class shooting : MonoBehaviour
{
    // gun stats
    public int damage;
    public float timeBetweenShooting, reloadSpeed, range, timeBetweenShots, spreadX, spreadY;
    public int magazineZise, bulletsPerShot;
    public bool automaticFire;
    int magazineRemainingAmmo, bulletsShot;
    bool firing, readyToFire, reloading;

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
    }
    private void Update()
    {
        //every frame checking for input
        GetInput();
    }
    private void GetInput()
    {

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
    }
    private void Shoot()
    {
        readyToFire = false;
        //spread
        float x = Random.Range(-spreadX, spreadX);
        float y = Random.Range(-spreadY, spreadY);
        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);
        //raycast using the random range from spread as 'direction'
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, enemy))
        {
            Debug.Log(rayHit.collider.name);
            Debug.DrawLine(transform.position, rayHit.point, Color.green, 1000f);
           
            TrailRenderer trail = Instantiate(bulletTracer, shootingPoint.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit));
            //if (rayHit.collider.CompareTag("Enemy"))
            //{
            //To hit an enemy they need to be tagged with enemy and also have a TakeDamage method
            //rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
            //}
        }
        
        //takes one bullet out of remaining ammo
        magazineRemainingAmmo--;
        //bullets shot count down by one so you can make guns using burst fire or shotgun blasts
        bulletsShot--;
        //Instantiate(muzzleFlash, shootingPoint);
        
        Debug.Log("Shot Fired");
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
        Debug.Log("Reloading");
        reloading = true;
        Invoke("ReloadFinished", reloadSpeed);
    }
    private void ReloadFinished()
    {
        Debug.Log("reload finished");
        //reload finished used with Invoke so different guns can have different reloading speeds
        magazineRemainingAmmo = magazineZise;
        reloading = false;
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        Debug.Log("start ienumerator");
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
        Instantiate(bulletHole, rayHit.point + (hit.normal * 0.1f), Quaternion.FromToRotation(Vector3.up, rayHit.normal));
        Destroy(Trail, time);
    }
}
