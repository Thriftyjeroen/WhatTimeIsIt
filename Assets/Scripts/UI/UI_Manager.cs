using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public Pausescreen Pausescreen; // Reference to the pause screen manager


    public void Button(string pScene)
    {
        // Load a new scene if it's not the currently active one
        if (SceneManager.GetActiveScene().name != pScene)
        {
            SceneManager.LoadScene(pScene);
        }
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    public void Continue()
    {
        // Resume the game by turning off the pause screen
        Pausescreen.PauseScreenOff();
    }
}
