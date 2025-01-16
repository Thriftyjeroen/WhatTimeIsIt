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
            SwitchWeapon(weezer, weezerAnimator, "Idle");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(nineBarrel, nineBarrelAnimator, "Idle");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWeapon(crossbow, crossbowAnimator, "Idle");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchWeapon(flintlock, flintlockAnimator, "Idle");
    }

    void SwitchWeapon(GameObject weapon, Animator animator, string defaultState)
    {
        DeactivateAllWeapons();

        // Reset animator state to idle
        ResetAnimatorToDefault(animator, defaultState);
        animator.SetTrigger("Idle");

        weapon.SetActive(true);
    }

    void DeactivateAllWeapons()
    {
        weezer.SetActive(false);
        nineBarrel.SetActive(false);
        crossbow.SetActive(false);
        flintlock.SetActive(false);
    }

    void ResetAnimatorToDefault(Animator animator, string defaultState)
    {
        if (animator != null)
        {
            animator.Play(defaultState); // Transition to the default state
            animator.Update(0);         // Apply the state immediately
        }
    }
}
