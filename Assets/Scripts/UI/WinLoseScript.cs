using TMPro;
using UnityEngine;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText;
    [SerializeField] GameObject PopUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // PopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Win() 
    {
        PopUp.SetActive(true);
        print("hi");
        WinLoseText.text = "you have won";
    }
    public void Lose()
    {
        PopUp.SetActive(true);
        print("you lose");
        WinLoseText.text = "you have lost";

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("finish"))
        {
            Win();
            Time.timeScale = 0f;
        }
        if (other.gameObject.name.ToLower().Contains("water"))
        {
            Lose();
            Time.timeScale = 0f; 

        }
    }
}
