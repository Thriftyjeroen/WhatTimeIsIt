using System.Runtime.CompilerServices;
using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;
    [SerializeField] Animator animatorFlintlock;
    private bool canShoot = true;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
                Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("shoot animation SHOULD play");
        canShoot = false;
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
        animatorFlintlock.SetTrigger("doodooCaca");
    }
    public void OnReloadComplete()
    {
        canShoot = true;
    }
}
