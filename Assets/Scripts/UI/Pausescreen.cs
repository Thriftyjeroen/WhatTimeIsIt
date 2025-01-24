using UnityEngine;

public class Pausescreen : MonoBehaviour
{
    [SerializeField] private GameObject pausescreen; // Pause screen UI element
    private bool isPaused; // Tracks whether the game is paused
    [SerializeField] private WinLoseScript winLoseScript; // Reference to WinLoseScript to check the game state

    void Start()
    {
        // Ensure the pause screen is hidden at the start
        pausescreen.SetActive(false);
    }

    void Update()
    {
        // Toggle the pause screen with Escape key if the game is not in a win/lose state
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
        // Show the pause screen, unlock cursor, and pause the game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausescreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void PauseScreenOff()
    {
        // Hide the pause screen, lock cursor, and resume the game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pausescreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
