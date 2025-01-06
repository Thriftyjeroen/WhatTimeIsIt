using UnityEngine;

public class scenemangerscript : MonoBehaviour
{
    private SceneSaver sceneSaver;

    void Start()
    {
        // Find the SceneSaver object that persists across scenes
        sceneSaver = FindObjectOfType<SceneSaver>();
    }

    public void LoadLastScene()
    {
        if (sceneSaver != null)
        {
            sceneSaver.loadLastScene(); // Call the method to load the last scene
        }
        else
        {
            Debug.LogError("SceneSaver not found!");
        }
    }
}
