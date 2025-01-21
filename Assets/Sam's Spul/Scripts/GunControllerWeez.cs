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
            Pew();
        }
    }

    void Pew()
    {
        canShoot = false;
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
        animatorFlintlock.SetTrigger("Reload");
    }
    public void OnReloadComplete()
    {
        canShoot = true;
    }
}
