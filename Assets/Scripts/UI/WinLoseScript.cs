using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText, scoretext;
    [SerializeField] GameObject PopUp;
    [SerializeField] ScoreUploadManager ScoreUploadManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Win()
    {
        StartCoroutine(HandleWin());
    }
    public void Lose()
    {
        PopUp.SetActive(true);
        WinLoseText.text = "you have lost";
        Time.timeScale = 0f;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("finish"))
        {
            ScoreUploadManager.UploadScore();
            Win();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        if (other.gameObject.name.ToLower().Contains("water"))
        {
            Lose();


        }
    }
    public void SwitchScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }



    IEnumerator HandleWin()
    {
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


