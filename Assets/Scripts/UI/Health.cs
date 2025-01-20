using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TMP_Text HealthText;
    [SerializeField] WinLoseScript WinLoseScript;
    int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setText();
        if(health == 0)
        {
            WinLoseScript.Lose();
        }
       
    }
    public void setText()
    {
        HealthText.text = "hitpoints: " + health.ToString();
    }
    public void TakeDamager(int damage)
    {
        health -= damage;
    }
}
