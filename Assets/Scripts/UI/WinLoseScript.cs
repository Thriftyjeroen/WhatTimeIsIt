using TMPro;
using UnityEngine;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Win() 
    {
        // when you win run this
        WinLoseText.text = "you have won";
    }
    public void Lose()
    {
        //when you lose run this
        WinLoseText.text = "you have lost";

    }
}
