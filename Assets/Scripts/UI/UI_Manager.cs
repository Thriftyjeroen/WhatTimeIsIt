using Unity.VisualScripting;
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
    public void Button2(int pScene) => SceneManager.LoadScene(pScene);

    public void Quit()
    {
        Application.Quit();
    }
}
