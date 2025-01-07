using TMPro;
using UnityEngine;

public class level_selector : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] GameObject PopUp;
    [SerializeField]TMP_Text leveltext;
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
        print("yes");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        

        if (collision.gameObject.name == "level_1")
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PopUp.SetActive(true);
            Debug.Log("LEVEL1 Collision detected!");
            levelname = "world1-1";
        }


    }
}
