using UnityEngine;
using UnityEngine.UI;

public class CurrentWeopon : MonoBehaviour
{
    [SerializeField] GameObject slot1background, slot2background, slot3background, slot4background; // Background GameObjects for inventory slots

    void Start()
    {
        // Find the GameObjects for each inventory slot background by name
        slot1background = GameObject.Find("slot 1 background");
        slot2background = GameObject.Find("slot 2 background");
        slot3background = GameObject.Find("slot 3 background");
        slot4background = GameObject.Find("slot 4 background");

        // Disable all backgrounds initially
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }

    public void slot1()
    {
        // Activate slot 1 background and disable others
        slot1background.SetActive(true);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }

    public void slot2()
    {
        // Activate slot 2 background and disable others
        slot1background.SetActive(false);
        slot2background.SetActive(true);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }

    public void slot3()
    {
        // Activate slot 3 background and disable others
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(true);
        slot4background.SetActive(false);
    }

    public void slot4()
    {
        // Activate slot 4 background and disable others
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(true);
    }
}
