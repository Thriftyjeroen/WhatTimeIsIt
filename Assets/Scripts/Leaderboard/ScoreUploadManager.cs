using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreUploadManager : MonoBehaviour
{
    public bool done = false;
    public bool failed = false;
    [SerializeField] TMP_Text fullScore;

    public void UploadScore()
    {
        done = false;
        failed = false;
        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        string pName = PlayerPrefs.GetString("Name");

        Scene scene = SceneManager.GetActiveScene();
        int pLevel = scene.buildIndex - 4;
        int pScore = int.Parse(fullScore.text);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            failed = true;
            yield break;
        }

        string request = "{ \"Name\": \"" + pName + "\", \"Score\": " + pScore + ", \"Level\": " + pLevel + "}";
        using (UnityWebRequest www = UnityWebRequest.Post("https://thriftyjeroen.nl/api/score", request, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                failed = true;
                yield break;
            }
            else
            {
                done = true;
            }
        }
    }
}
