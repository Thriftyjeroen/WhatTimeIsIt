using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] GameObject weezer;
    [SerializeField] GameObject nineBarrel;
    [SerializeField] GameObject crossbow;
    [SerializeField] GameObject flintlock;

    Animator weezerAnimator, nineBarrelAnimator, crossbowAnimator, flintlockAnimator;

    void Start()
    {
        // Initialize Animators
        weezerAnimator = weezer.GetComponent<Animator>();
        nineBarrelAnimator = nineBarrel.GetComponent<Animator>();
        crossbowAnimator = crossbow.GetComponent<Animator>();
        flintlockAnimator = flintlock.GetComponent<Animator>();

        DeactivateAllWeapons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(weezer, weezerAnimator);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(nineBarrel, nineBarrelAnimator);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWeapon(crossbow, crossbowAnimator);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchWeapon(flintlock, flintlockAnimator);
    }

    void SwitchWeapon(GameObject weapon, Animator animator)
    {
        DeactivateAllWeapons();

        // Stop ongoing animations to prevent freeze
        ResetAnimator(animator);

        weapon.SetActive(true);
    }

    void DeactivateAllWeapons()
    {
        weezer.SetActive(false);
        nineBarrel.SetActive(false);
        crossbow.SetActive(false);
        flintlock.SetActive(false);
    }

    void ResetAnimator(Animator animator)
    {
        if (animator != null)
        {
            animator.Rebind();  // Resets the Animator to its default state
            animator.Update(0); // Apply the reset immediately
        }
    }
}
