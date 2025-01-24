using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText, scoretext; // Text components for win/lose message and score
    [SerializeField] GameObject PopUp; // Popup UI element
    [SerializeField] ScoreUploadManager ScoreUploadManager; // Reference to score upload manager
    public bool active; // Tracks whether the game is in a win/lose state

    public void Start()
    {
        // Hide the popup at the start of the game
        PopUp.SetActive(false);
    }

    public void Win()
    {
        // Freeze time and handle win logic
        Time.timeScale = 0f;
        StartCoroutine(HandleWin());
    }

    public void Lose()
    {
        // Handle lose logic: show popup, unlock cursor, display score, and freeze time
        active = true;
        PopUp.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        scoretext.text = ScoreUploadManager.fullScore.text;
        WinLoseText.text = "You have lost";
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has reached the finish
        string objectName = other.gameObject.name.ToLower();
        if (objectName.Contains("finish"))
        {
            ScoreUploadManager.UploadScore(); // Start score upload
            Win(); // Trigger win logic
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SwitchScene()
    {
        // Switch to the level selection scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }

    public void ResetScene()
    {
        // Reload the current scene
        Scene activescene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activescene.name);
    }

    IEnumerator HandleWin()
    {
        active = true;
        // Start uploading the score
        ScoreUploadManager.UploadScore();

        // Wait until the upload is finished
        while (!ScoreUploadManager.done && !ScoreUploadManager.failed)
        {
            yield return null; // Wait for the next frame
        }

        // Show the popup and set win/lose text based on upload result
        PopUp.SetActive(true);
        scoretext.text = ScoreUploadManager.fullScore.text;

        if (ScoreUploadManager.failed)
        {
            WinLoseText.text = "You have won, but the score upload failed.";
        }
        else
        {
            WinLoseText.text = "You have won, and your score was uploaded.";
        }
    }
}
