using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScript : MonoBehaviour
{
    [SerializeField] TMP_Text WinLoseText,scoretext;
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
        PopUp.SetActive(true);
        scoretext.text = ScoreUploadManager.fullScore.text;
        WinLoseText.text = "you have won";
    }
    public void Lose()
    {
        PopUp.SetActive(true);
        WinLoseText.text = "you have lost";

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("finish"))
        {
            ScoreUploadManager.UploadScore();
            Win();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        if (other.gameObject.name.ToLower().Contains("water"))
        {
            Lose();
            Time.timeScale = 0f; 

        }
    }
    public void SwitchScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
