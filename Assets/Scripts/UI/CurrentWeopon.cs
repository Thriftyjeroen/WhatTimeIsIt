using UnityEngine;
using UnityEngine.UI;

public class CurrentWeopon : MonoBehaviour
{
    [SerializeField] GameObject slot1background, slot2background, slot3background, slot4background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slot1background = GameObject.Find("slot 1 background");
        slot2background = GameObject.Find("slot 2 background");
        slot3background = GameObject.Find("slot 3 background");
        slot4background = GameObject.Find("slot 4 background");
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
        print("SLOT1");
        slot1background.SetActive (true);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(false);
    }
    public void slot2()
    {
        print("SLOT2");
        slot1background.SetActive (false);
        slot2background.SetActive (true);
        slot3background.SetActive (false);
        slot4background.SetActive (false);
    }
    public void slot3()
    {
        print("SLOT3");
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(true);
        slot4background.SetActive(false);
    }
    public void slot4()
    {
        print("SLOT4");
        slot1background.SetActive(false);
        slot2background.SetActive(false);
        slot3background.SetActive(false);
        slot4background.SetActive(true);
    }
}
