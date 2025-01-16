using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level_selector : MonoBehaviour
{
    [SerializeField] CapsuleCollider player;
    [SerializeField] GameObject PopUp;
    [SerializeField] TMP_Text leveltext;
    string levelname;

    void Start()
    {

        if (PopUp == null)
        {
            Debug.LogError("PopUp is not assigned in the Inspector!");
            return;
        }

        PopUp.SetActive(false);

        // Try to find TMP_Text component
        leveltext = PopUp.transform.Find("level-text")?.GetComponent<TMP_Text>();
        if (leveltext == null)
        {
            Debug.LogError("TMP_Text component not found on PopUp or its children!");
        }
    }

    void Update()
    {
        if (leveltext != null)
        {
            leveltext.text = levelname;
        }
        else
        {
            Debug.LogWarning("leveltext is still null. Check if TMP_Text exists under PopUp.");
        }
    }
    public void AnswerNo()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUp.SetActive(false);
    }
    public void AnswerYes()
    {
        //send you to the level
        Time.timeScale = 1.0f;
        if(levelname == "world 1-1")
        {
            SceneManager.LoadScene("buildLevel");
        }
            SceneManager.LoadScene(levelname);
        
    }
    private void ShowPopup(string level)
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PopUp.SetActive(true);
        levelname = level;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "level_1")
        {
            ShowPopup("world 1-1");
        }
        // Trigger for level 2
        if (collision.gameObject.name == "level_2")
        {
            ShowPopup("world 1-2");
        }
    }
}
