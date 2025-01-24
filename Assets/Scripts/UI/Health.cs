using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TMP_Text HealthText; // Text component to display health
    [SerializeField] WinLoseScript WinLoseScript; // Reference to WinLoseScript for handling loss
    int health = 100; // Initial health value

    void Update()
    {
        // Update the health text
        setText();

        // Check if health is zero or below and handle game loss
        if (health <= 0)
        {
            WinLoseScript.Lose();
            Time.timeScale = 0f;
        }
    }

    public void setText()
    {
        // Update the health display text
        HealthText.text = "hitpoints: " + health.ToString();
    }

    public void TakeDamager(int damage)
    {
        // Reduce health by the damage amount
        health -= damage;
    }
}
