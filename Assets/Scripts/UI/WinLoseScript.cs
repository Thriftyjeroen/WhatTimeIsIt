using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText, scoretext;
    [SerializeField] GameObject PopUp;
    [SerializeField] ScoreUploadManager ScoreUploadManager;
    public bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        PopUp.SetActive(false);
    }
    public void Win()
    {
        // Set time on 0 
        Time.timeScale = 0f;
        StartCoroutine(HandleWin());
  

    }
    public void Lose()
    {
            active = true;
            PopUp.SetActive(true);
            scoretext.text = ScoreUploadManager.fullScore.text;
            WinLoseText.text = "You have lost";
            Time.timeScale = 0f;
           
    }
    private void OnTriggerEnter(Collider other)
    {
        string objectName = other.gameObject.name.ToLower();

        if (objectName.Contains("finish"))
        {
            ScoreUploadManager.UploadScore();
            Win();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (objectName.Contains("water"))
        {
            ScoreUploadManager.UploadScore();
            Lose();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }


    public void SwitchScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }
    public void ResetScene()
    {
        Scene activescene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activescene.name);
    }



    IEnumerator HandleWin()
    {

        active = true;
        // Start uploading the score
        ScoreUploadManager.UploadScore();

        // Wait for the upload to complete
        while (!ScoreUploadManager.done && !ScoreUploadManager.failed)
        {
            yield return null; // Wait for the next frame
        }

        // Now show the popup
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


