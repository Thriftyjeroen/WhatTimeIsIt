using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject GoalText; // UI element to display goal text
    float Time; // Timer to track how long the goal text is visible

    void Start()
    {
        // No initialization needed for now
    }

    void Update()
    {
        // Increment the timer while it's less than or equal to 5 seconds
        if (Time <= 5)
        {
            Time += UnityEngine.Time.deltaTime;
        }

        // Hide the goal text once the timer exceeds 5 seconds
        if (Time >= 5)
        {
            GoalText.SetActive(false);
        }
    }
}
