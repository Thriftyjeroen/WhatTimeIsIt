using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GetLeaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text board; // Text element to display leaderboard
    List<ScoreEntry> scores; // List to hold score entries
    [SerializeField] int level; // Level to fetch the leaderboard for
    [SerializeField] string playerName; // Player's name to show their rank

    public void Start()
    {
        playerName = PlayerPrefs.GetString("Name"); // Get player's name from preferences
        ShowScoreBoard(playerName, level); // Display scoreboard
    }

    // Show the scoreboard with player's name and level
    public void ShowScoreBoard(string pName, int pLevel)
    {
        IEnumerator func1 = LoadCircle(); // Start loading circle (simulating loading)
        StartCoroutine(func1);
        board.text = "Scoreboard"; // Show scoreboard title
        IEnumerator func = Request(pName, pLevel); // Start request to get leaderboard data
        StartCoroutine(func);
    }

    // Request the leaderboard data from server
    IEnumerator Request(string pName, int pLevel)
    {
        board.text = "Scoreboard"; // Display title
        using (UnityWebRequest www = UnityWebRequest.Get($"https://thriftyjeroen.nl/api/score/board?level={pLevel}"))
        {
            yield return www.SendWebRequest(); // Send request to server

            // Handle connection errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error); // Log error
                board.text += "\n\nCannot connect to leaderboard server."; // Display error
                yield break; // Stop the function if there's an error
            }

            // Deserialize the JSON response into ScoreWrapper
            ScoresWrapper scoresWrapped = JsonUtility.FromJson<ScoresWrapper>(www.downloadHandler.text);
            List<ScoreEntry> scores = scoresWrapped.scores; // Get the list of scores

            // Display top 10 scores or fewer if less than 10 entries
            for (int j = 0; j < 10 && j < scores.Count; j++)
            {
                board.text = board.text + $"\n===={j + 1}====\n{scores[j].name} - {scores[j].score}";
            }

            // Show the player's score and position if they're in the list
            for (int j = 0; j < scores.Count; j++)
            {
                if (scores[j].name == pName)
                {
                    board.text = board.text + $"\n\n==============\nYou are in place {j + 1}\n{scores[j].name} - {scores[j].score}";
                }
            }
        }
    }

    // A dummy loading circle function to simulate waiting
    IEnumerator LoadCircle()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f); // Simulate delay
        }
    }
}

// Score entry model to hold player name and score
[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
    public int level;
}

// Wrapper to hold a list of score entries
[System.Serializable]
public class ScoresWrapper
{
    public List<ScoreEntry> scores;
}
