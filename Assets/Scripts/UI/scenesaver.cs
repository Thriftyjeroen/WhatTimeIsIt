using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaver : MonoBehaviour
{
    private string lastSceneName;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        lastSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
    }

    public void loadLastScene()
    {
        if (!string.IsNullOrEmpty(lastSceneName))
        {
            Debug.Log("Loading scene: " + lastSceneName);
            SceneManager.LoadScene(lastSceneName);
        }
        else
        {
            Debug.Log("No previous scene saved.");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
