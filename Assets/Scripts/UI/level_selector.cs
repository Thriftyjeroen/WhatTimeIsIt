using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level_selector : MonoBehaviour
{
    [SerializeField] CapsuleCollider player; // Reference to the player's collider
    [SerializeField] GameObject PopUp; // Popup UI for level selection
    [SerializeField] TMP_Text leveltext; // Text to display the level name
    string levelname; // Name of the level to load

    void Start()
    {
        // Ensure PopUp is assigned
        if (PopUp == null)
        {
            Debug.LogError("PopUp is not assigned in the Inspector!");
            return;
        }

        // Hide the popup at the start
        PopUp.SetActive(false);

        // Try to find the TMP_Text component in PopUp's children
        leveltext = PopUp.transform.Find("level-text")?.GetComponent<TMP_Text>();
        if (leveltext == null)
        {
            Debug.LogError("TMP_Text component not found on PopUp or its children!");
        }
    }

    void Update()
    {
        // Update the level name text if the TMP_Text is assigned
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
        // Resume the game and hide the popup
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUp.SetActive(false);
    }

    public void AnswerYes()
    {
        // Load the selected level
        if (levelname == "world 1-1")
        {
            print(levelname + " is loading");
            SceneManager.LoadScene("buildLevel");
            Time.timeScale = 1.0f;
        }
    }

    private void ShowPopup(string level)
    {
        // Show the popup and update the level name
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PopUp.SetActive(true);
        levelname = level;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Show the popup for level 1
        if (collision.gameObject.name == "level_1")
        {
            ShowPopup("world 1-1");
        }

    
    }
}
