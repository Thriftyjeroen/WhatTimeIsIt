using TMPro;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int fullScore = 0;
    [SerializeField] private int scoreToAdd = 0;
    [SerializeField] private TMP_Text fullScoreText;
    [SerializeField] private TMP_Text tempScoreText;
    [SerializeField] GameObject multIncreasePlayer;
    private int chips = 0;
    [SerializeField] private float mult = 1;

    private Coroutine countDownCoroutine;

    private void Start()
    {
        StartCoroutine(KeepAddingScore());
        StartCoroutine(KeepDecayingScore());
    }

    public void DamageDone(int damage)
    {
        chips += damage * 100;

        // If currently adding score, finalize it and start fresh
        FinalizeOngoingScore();
        StartCountDown();

        UpdateTempScoreText();
    }

    public void IncreaseMult(float amount)
    {
        mult += amount;
        Instantiate(multIncreasePlayer);

        // If currently adding score, finalize it and start fresh
        FinalizeOngoingScore();
        StartCountDown();

        UpdateTempScoreText();
    }

    public void FinalizeOngoingScore()
    {
        // Immediately add any score left to `fullScore`
        if (scoreToAdd > 0)
        {
            fullScore += scoreToAdd;
            fullScoreText.text = fullScore.ToString();
            scoreToAdd = 0;
        }
    }

    private void StartCountDown()
    {
        if (countDownCoroutine != null)
        {
            StopCoroutine(countDownCoroutine);
        }
        countDownCoroutine = StartCoroutine(CountDown());
    }

    private void UpdateTempScoreText()
    {
        tempScoreText.text = $"{chips} x {mult}";
    }

    private IEnumerator KeepDecayingScore()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            if (fullScore > 0)
            {
                fullScore -= 1;
                fullScoreText.text = fullScore.ToString();
            }
        }
    }
    private IEnumerator CountDown()
    {
        // Wait 5 seconds before calculating scoreToAdd
        yield return new WaitForSecondsRealtime(5f);

        scoreToAdd = Mathf.RoundToInt(chips * mult);
        chips = 0;
        mult = 1;
        tempScoreText.text = scoreToAdd.ToString();
    }

    private IEnumerator KeepAddingScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (scoreToAdd > 0)
            {
                // Scale the increment based on scoreToAdd size
                int increment = Mathf.CeilToInt(scoreToAdd * 0.1f); // Adjust increment proportionally
                increment = Mathf.Max(increment, 1); // Ensure at least 1 is added per iteration

                // Transfer score to fullScore
                fullScore += increment;
                scoreToAdd -= increment;

                // Update UI
                fullScoreText.text = fullScore.ToString();
                tempScoreText.text = scoreToAdd.ToString();
            }
        }
    }
}
