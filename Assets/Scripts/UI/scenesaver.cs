using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaver : MonoBehaviour
{
    private string lastSceneName;
    private string previousSceneName; // To store the scene before the last one
    private static SceneSaver instance;

    void Awake()
    {
        // Ensure that only one instance of SceneSaver exists and persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance if one already exists
        }
    }
    void Start()
    {
        
 
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
