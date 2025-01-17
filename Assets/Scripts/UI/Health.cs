using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TMP_Text HealthText;
    int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setText();
       
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
