using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    // References to the animator components
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;
    [SerializeField] Animator animatorFlintlock;
    private bool canShoot = true;
    private bool canAbility = true;

    void Update()
    {
        // Check for left mouse button press to trigger shooting, only if shooting is allowed
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Pew();
        }
        // Check for right mouse button press to trigger abilities, only if abilities are allowed
        if (Input.GetMouseButtonDown(1) && canAbility)
        {
            Ability();
        }
    }

    void Pew()
    {
        // Disable shooting and abilities during the shooting animation
        canShoot = false;
        canAbility = false;
        // Trigger the reload animation for all weapon animators
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
        animatorFlintlock.SetTrigger("Reload");
    }
    void Ability()
    {
        // Disable abilities and shooting during the ability animation
        canAbility = false;
        canShoot = false;
        animatorGun2.SetTrigger("Ability");
        animatorCrossbow.SetTrigger("Ability");
        animatorFlintlock.SetTrigger("Ability");
    }
    public void OnReloadComplete()
    {
        // reset shooting and ability when the reload animation completes
        canShoot = true;
        canAbility = true;
    }
}
