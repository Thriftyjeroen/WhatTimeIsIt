using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaver : MonoBehaviour
{
    private string lastSceneName; // Stores the name of the last loaded scene
    private string previousSceneName; // Stores the name of the scene before the last one
    private static SceneSaver instance; // Singleton instance to ensure only one SceneSaver exists

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
        // Initialize scene tracking with the current active scene
        lastSceneName = SceneManager.GetActiveScene().name;
        previousSceneName = lastSceneName; // Initially, both are the same
    }

    public void loadLastScene()
    {
        // Load the previous scene if it exists
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
        // Update scene tracking when a new scene is loaded
        Time.timeScale = 1.0f;
        previousSceneName = lastSceneName;
        lastSceneName = scene.name;
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
