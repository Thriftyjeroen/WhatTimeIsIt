using UnityEngine;

public class Pausescreen : MonoBehaviour
{
    [SerializeField] private GameObject pausescreen; // Ensure this is assigned in the inspector
    private bool isPaused;
    [SerializeField] private WinLoseScript winLoseScript;  // Reference to WinLoseScript to check the game state

    void Start()
    {
        pausescreen.SetActive(false);
    }

    void Update()
    {
        // Only allow the pause screen if the game is not in a win/lose state (i.e., Active is false)
        if (Input.GetKeyDown(KeyCode.Escape) && winLoseScript.active == false)
        {
            if (isPaused)
            {
                PauseScreenOff();
            }
            else
            {
                PauseScreenOn();
            }
        }
    }

    public void PauseScreenOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausescreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true; // Set paused state
    }

    public void PauseScreenOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pausescreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false; // Reset paused state
    }
}
