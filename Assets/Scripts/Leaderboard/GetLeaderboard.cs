using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Video;

public class GetLeaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text board;
    List<ScoreEntry> scores;
    // [SerializeField] GameObject loadingCircle;
    [SerializeField] int level;
    [SerializeField] string playerName;

    public void Start()
    {
        playerName = PlayerPrefs.GetString("Name");
        ShowScoreBoard(playerName, level);
    }

    public void ShowScoreBoard(string pName, int pLevel)
    {
        IEnumerator func1 = LoadCircle();
        StartCoroutine(func1);
        board.text = "Scoreboard";
        IEnumerator func = Request(pName, pLevel);
        StartCoroutine(func);
    }
    IEnumerator Request(string pName, int pLevel)
    {
        board.text = "Scoreboard";
        using (UnityWebRequest www = UnityWebRequest.Get($"https://thriftyjeroen.nl/api/score/board?level={pLevel}"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                board.text += "\n\nCannot connect to leaderboard server.";
                yield break;
            }

            ScoresWrapper scoresWrapped = JsonUtility.FromJson<ScoresWrapper>(www.downloadHandler.text);
            List<ScoreEntry> scores = scoresWrapped.scores;

            for (int j = 0; j < 10 && j < scores.Count; j++) // Repeats 10 times or when file ends
            {
                board.text = board.text + $"\n===={j + 1}====\n{scores[j].name} - {scores[j].score}";
            }

            for (int j = 0; j < scores.Count; j++)
            {
                if (scores[j].name == pName)
                {
                    board.text = board.text + $"\n\n==============\nYou are in place {j + 1}\n{scores[j].name} - {scores[j].score}";
                }
            }
        }
    }

    IEnumerator LoadCircle()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            //loadingCircle.SetActive((board.text == "Scoreboard"));
        }
    }
}


[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
    public int level;
}

[System.Serializable]
public class ScoresWrapper
{
    public List<ScoreEntry> scores;
}