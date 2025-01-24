using System.Runtime.CompilerServices;
using UnityEngine;

public class GunControllerWeez : MonoBehaviour
{
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;
    [SerializeField] Animator animatorFlintlock;
    private bool canShoot = true;
    private bool canAbility = true;

    void Update()
    {
        // Check for shooting input
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Pew();
        }
        // Check for ability input
        if (Input.GetMouseButtonDown(1) && canAbility)
        {
            Ability();
        }
    }

    void Pew()
    {
        // Prevent further shooting and ability activation during reload
        canShoot = false;
        canAbility = false;

        // Trigger reload animations for all weapons
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
        animatorFlintlock.SetTrigger("Reload");
    }

    void Ability()
    {
        // Prevent further shooting and ability activation during ability use
        canAbility = false;
        canShoot = false;

        // Trigger ability animations for all weapons
        animatorGun2.SetTrigger("Ability");
        animatorCrossbow.SetTrigger("Ability");
        animatorFlintlock.SetTrigger("Ability");
    }

    // Called when reload is completed
    public void OnReloadComplete()
    {
        // Allow shooting and ability use again
        canShoot = true;
        canAbility = true;
    }
}
