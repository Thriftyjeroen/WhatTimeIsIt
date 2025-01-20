using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject GoalText;
    float Time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time <= 5)
        {
            Time += UnityEngine.Time.deltaTime;
        }
        if (Time >= 5)
        {
            GoalText.SetActive(false);
        }
    }
}
