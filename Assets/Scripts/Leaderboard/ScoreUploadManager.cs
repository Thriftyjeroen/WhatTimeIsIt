using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreUploadManager : MonoBehaviour
{
    public bool done = false; // Flag to check if the upload is successful
    public bool failed = false; // Flag to check if the upload failed
    public TMP_Text fullScore; // Text UI to display the player's full score

    // Method to upload the score
    public void UploadScore()
    {
        done = false; // Reset upload status
        failed = false; // Reset failure status
        StartCoroutine(Request()); // Start the coroutine to upload score
    }

    // Coroutine to handle the HTTP request
    IEnumerator Request()
    {
        // Get the player's name from PlayerPrefs
        string pName = PlayerPrefs.GetString("Name");

        // Get the current scene and calculate the player's level based on scene index
        Scene scene = SceneManager.GetActiveScene();
        int pLevel = scene.buildIndex - 4; // Assuming levels start from 4

        // Get the player's score from the UI text
        int pScore = int.Parse(fullScore.text);

        // Check if there is no internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            failed = true; // Mark as failed if no connection
            yield break; // Exit the coroutine
        }

        // Create the request JSON string with player's details
        string request = "{ \"Name\": \"" + pName + "\", \"Score\": " + pScore + ", \"Level\": " + pLevel + "}";

        // Send the POST request to upload the score
        using (UnityWebRequest www = UnityWebRequest.Post("https://thriftyjeroen.nl/api/score", request, "application/json"))
        {
            // Wait for the request to finish
            yield return www.SendWebRequest();

            // Check if the request was successful
            if (www.result != UnityWebRequest.Result.Success)
            {
                failed = true; // Mark as failed if there was an error
                yield break; // Exit the coroutine
            }
            else
            {
                done = true; // Mark as successful if the request succeeded
            }
        }
    }
}
