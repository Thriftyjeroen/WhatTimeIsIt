using UnityEngine;
using UnityEngine.UI;

public class CurrentWeopon : MonoBehaviour
{
    [SerializeField] GameObject slot1background, slot2background, slot3background, slot4background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void slot1()
    {
        slot1background.SetActive (true);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }
    public void slot2()
    {
        slot1background.SetActive (false);
        slot2background.SetActive (true);
        slot3background.SetActive (false);
        slot4background.SetActive (false);
    }
    public void slot3()
    {
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(true);
        slot4background.SetActive(false);
    }
    public void slot4()
    {
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(true);
    }
}
