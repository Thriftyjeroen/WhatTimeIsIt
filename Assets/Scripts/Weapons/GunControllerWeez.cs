using System.Runtime.CompilerServices;
using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;
    [SerializeField] Animator animatorFlintlock;
    private bool canShoot = true;
    private bool canAbility = true;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Pew();
        }
        if (Input.GetMouseButtonDown(1) && canAbility)
        {
            Ability();
        }
    }

    void Pew()
    {
        canShoot = false;
        canAbility = false;
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
        animatorFlintlock.SetTrigger("Reload");
    }
    void Ability()
    {
        canAbility = false;
        canShoot = false;
        animatorGun2.SetTrigger("Ability");
        animatorCrossbow.SetTrigger("Ability");
        animatorFlintlock.SetTrigger("Ability");
    }
    public void OnReloadComplete()
    {
        canShoot = true;
        canAbility = true;
    }
}
