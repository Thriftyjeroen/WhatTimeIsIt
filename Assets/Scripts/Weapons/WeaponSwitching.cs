using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] GunControllerWeez gunControllerWeez; // Reference to the GunControllerWeez to handle weapon actions

    [SerializeField] GameObject weezer; // Reference to the weezer weapon GameObject
    [SerializeField] GameObject nineBarrel; // Reference to the nineBarrel weapon GameObject
    [SerializeField] GameObject crossbow; // Reference to the crossbow weapon GameObject
    [SerializeField] GameObject flintlock; // Reference to the flintlock weapon GameObject

    [SerializeField] Animator weezerAnimator; // Animator for the weezer weapon
    [SerializeField] Animator nineBarrelAnimator; // Animator for the nineBarrel weapon
    [SerializeField] Animator crossbowAnimator; // Animator for the crossbow weapon
    [SerializeField] Animator flintlockAnimator; // Animator for the flintlock weapon
    [SerializeField] CurrentWeopon weopon; // Reference to manage the current weapon slot

    private void Start()
    {
        // Initially deactivate all weapons
        weezer.SetActive(false);
        nineBarrel.SetActive(false);
        flintlock.SetActive(false);
        crossbow.SetActive(false);
    }

    private void Update()
    {
        // Switch to the first weapon (weezer) when the "1" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Set the appropriate weapon to active and set others to inactive
            weezerAnimator.Play("Idle"); // Play idle animation for weezer
            weezer.SetActive(true);
            nineBarrel.SetActive(false);
            flintlock.SetActive(false);
            crossbow.SetActive(false);
            weopon.slot1(); // Update the weapon slot
            gunControllerWeez.OnReloadComplete(); // Call reload complete function
        }

        // Switch to the second weapon (nineBarrel) when the "2" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weezerAnimator.Play("Idle"); // Play idle animation for weezer
            weezer.SetActive(false);
            nineBarrel.SetActive(true);
            flintlock.SetActive(false);
            crossbow.SetActive(false);
            weopon.slot2(); // Update the weapon slot
            gunControllerWeez.OnReloadComplete(); // Call reload complete function
        }

        // Switch to the third weapon (crossbow) when the "3" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weezerAnimator.Play("Idle"); // Play idle animation for weezer
            weezer.SetActive(false);
            nineBarrel.SetActive(false);
            flintlock.SetActive(false);
            crossbow.SetActive(true);
            weopon.slot3(); // Update the weapon slot
            gunControllerWeez.OnReloadComplete(); // Call reload complete function
        }

        // Switch to the fourth weapon (flintlock) when the "4" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weezerAnimator.Play("Idle"); // Play idle animation for weezer
            weezer.SetActive(false);
            nineBarrel.SetActive(false);
            flintlock.SetActive(true);
            crossbow.SetActive(false);
            weopon.slot4(); // Update the weapon slot
            gunControllerWeez.OnReloadComplete(); // Call reload complete function
        }
    }
}
