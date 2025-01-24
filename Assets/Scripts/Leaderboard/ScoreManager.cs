using TMPro;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int fullScore = 0; // Total score displayed
    [SerializeField] private int scoreToAdd = 0; // Temporary score to be added
    [SerializeField] private TMP_Text fullScoreText; // Text object to show full score
    [SerializeField] private TMP_Text tempScoreText; // Text object to show temporary score
    [SerializeField] GameObject multIncreasePlayer; // Object to show when multiplier increases
    private int chips = 0; // The score increment based on damage
    [SerializeField] private float mult = 1; // The multiplier for score calculation

    private Coroutine countDownCoroutine; // Coroutine for countdown to finalize score

    private void Start()
    {
        StartCoroutine(KeepAddingScore()); // Start adding score to full score
        StartCoroutine(KeepDecayingScore()); // Start decaying score over time
    }

    public float GetMult()
    {
        return mult; // Return the current multiplier
    }

    public void IncreaseScore(int damage)
    {
        chips += damage * 10; // Add score based on damage (multiplied by 10)

        // Finalize ongoing score and start countdown to add the score
        FinalizeOngoingScore();
        StartCountDown();

        UpdateTempScoreText(); // Update the temporary score display
    }

    public void IncreaseMult(float amount)
    {
        mult += amount; // Increase the multiplier by a specified amount
        Instantiate(multIncreasePlayer); // Show the multiplier increase effect

        // Finalize ongoing score and start countdown to add the score
        FinalizeOngoingScore();
        StartCountDown();

        UpdateTempScoreText(); // Update the temporary score display
    }

    public void FinalizeOngoingScore()
    {
        // Immediately add any remaining score to `fullScore` and update the UI
        if (scoreToAdd > 0)
        {
            fullScore += scoreToAdd;
            fullScoreText.text = fullScore.ToString(); // Update the full score display
            scoreToAdd = 0; // Reset temporary score
        }
    }

    private void StartCountDown()
    {
        // Stop any ongoing countdown and start a new one
        if (countDownCoroutine != null)
        {
            StopCoroutine(countDownCoroutine);
        }
        countDownCoroutine = StartCoroutine(CountDown());
    }

    private void UpdateTempScoreText()
    {
        // Update the temporary score display with current chips and multiplier
        tempScoreText.text = $"{chips} x {mult}";
    }

    private IEnumerator KeepDecayingScore()
    {
        // Continuously decrease the full score slightly over time
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f); // Wait 0.1 seconds between updates
            if (fullScore > 0 && Time.timeScale > 0f)
            {
                fullScore -= 1; // Decrease the full score by 1
                fullScoreText.text = fullScore.ToString(); // Update the full score display
            }
        }
    }

    private IEnumerator CountDown()
    {
        // Wait for 5 seconds before finalizing the score to add
        yield return new WaitForSecondsRealtime(5f);

        scoreToAdd = Mathf.RoundToInt(chips * mult); // Calculate the final score to add
        chips = 0; // Reset chips
        mult = 1; // Reset multiplier
        tempScoreText.text = scoreToAdd.ToString(); // Update temporary score display
    }

    private IEnumerator KeepAddingScore()
    {
        // Continuously add the score over time
        while (true)
        {
            yield return new WaitForSeconds(0.01f); // Wait 0.01 seconds between increments

            if (scoreToAdd > 0)
            {
                // Scale the increment based on the size of `scoreToAdd`
                int increment = Mathf.CeilToInt(scoreToAdd * 0.1f); // Proportional increment
                increment = Mathf.Max(increment, 1); // Ensure at least 1 score is added each time

                fullScore += increment; // Add the increment to the full score
                scoreToAdd -= increment; // Decrease the remaining score to add

                // Update UI with the new full score and temporary score
                fullScoreText.text = fullScore.ToString();
                tempScoreText.text = scoreToAdd.ToString();
            }
        }
    }
}
