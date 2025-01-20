using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour //if the player reach the end of the level the next scene will be loaded
{
    [SerializeField] string loadScene;
    [SerializeField] ScoreUploadManager scoreUploadManager;
    private void Start()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.name.ToLower().Contains("player")) SceneManager.LoadScene(loadScene);

        if (other.gameObject.CompareTag("Player"))
        {
            scoreUploadManager.UploadScore();
            SceneManager.LoadScene(loadScene);
        }
    }
}