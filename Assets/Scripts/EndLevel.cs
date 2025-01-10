using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour //if the player reach the end of the level the next scene will be loaded
{
    [SerializeField] string loadScene;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "player") SceneManager.LoadScene(loadScene);
    }
}
