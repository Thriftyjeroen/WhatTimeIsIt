using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetUIElement(int Number)
        
    {
        switch (Number)
        {
            case 1:
                SceneManager.LoadScene("");
            break;
            case 2:
                SceneManager.LoadScene("");
                break;
            case 3:
                SceneManager.LoadScene("");
                break;
            case 4:
                SceneManager.LoadScene("");
                break;
            case 5:
                Application.Quit();
                break;
        }

    }
}
