using UnityEngine;

public class Pausescreen : MonoBehaviour
{
    [SerializeField] GameObject pausescreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausescreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreenOn();
        }
    }
    public void PauseScreenOn()
    { 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausescreen.SetActive(true);
       
    }
    public void PauseScreenOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pausescreen.SetActive(false);
    }
}
