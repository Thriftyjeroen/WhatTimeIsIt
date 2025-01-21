    using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] GunControllerWeez gunControllerWeez;

    [SerializeField] GameObject weezer;
    [SerializeField] GameObject nineBarrel;
    [SerializeField] GameObject crossbow;
    [SerializeField] GameObject flintlock;

    [SerializeField] Animator weezerAnimator;
    [SerializeField] Animator nineBarrelAnimator;
    [SerializeField] Animator crossbowAnimator;
    [SerializeField] Animator flintlockAnimator;

    private void Start()
    {
        weezer.SetActive(false);
        nineBarrel.SetActive(false);
        flintlock.SetActive(false);
        crossbow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weezerAnimator.Play("Idle");
            weezer.SetActive(true);
            nineBarrel.SetActive(false);
            flintlock.SetActive(false);
            crossbow.SetActive(false);
            gunControllerWeez.OnReloadComplete();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weezerAnimator.Play("Idle");
            weezer.SetActive(false);
            nineBarrel.SetActive(true);
            flintlock.SetActive(false);
            crossbow.SetActive(false);
            gunControllerWeez.OnReloadComplete();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weezerAnimator.Play("Idle");
            weezer.SetActive(false);
            nineBarrel.SetActive(false);
            flintlock.SetActive(false);
            crossbow.SetActive(true);
            gunControllerWeez.OnReloadComplete();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weezerAnimator.Play("Idle");
            weezer.SetActive(false);
            nineBarrel.SetActive(false);
            flintlock.SetActive(true);
            crossbow.SetActive(false);
            gunControllerWeez.OnReloadComplete();
        }
    }
}
