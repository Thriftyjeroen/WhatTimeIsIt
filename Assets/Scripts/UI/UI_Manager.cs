using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public Pausescreen Pausescreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Button(string pScene)
    {
        if (SceneManager.GetActiveScene().name != pScene) // Avoid reloading the current scene
        {
            SceneManager.LoadScene(pScene);
        }
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Continue()
    {
        Pausescreen.PauseScreenOff();
    }
}
