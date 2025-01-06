using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaver : MonoBehaviour
{
    private string lastSceneName;
    private string previousSceneName; // To store the scene before the last one

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        lastSceneName = SceneManager.GetActiveScene().name;
        previousSceneName = lastSceneName; // Initially, it's the same as the current scene
    }
    

    public void loadLastScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            Debug.Log("Loading last scene: " + previousSceneName);
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.Log("No previous scene saved.");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        previousSceneName = lastSceneName;
        lastSceneName = scene.name; 
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
