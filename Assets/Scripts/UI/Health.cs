using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TMP_Text HealthText;
    int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Plushealth();
        }
        setText();
    }
    public void Plushealth()
    {
        health += 1;
    }
    public void setText()
    {
        HealthText.text = "health: " + health.ToString();
    }
}
