using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class NameInputManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Text unavailableText;

    public void Start()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        nameInput.characterLimit = 15;
    }

    public void StartCrusadeStarter()
    {
        StartCoroutine(StartCrusade());
    }
    public IEnumerator StartCrusade()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"https://thriftyjeroen.nl/api/score/playerAvailable?name={nameInput.text}"))
        {
            yield return www.SendWebRequest();
            bool available = System.Convert.ToBoolean(www.downloadHandler.text);

            if (available)
            {
                PlayerPrefs.SetString("Name", nameInput.text);
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                StartCoroutine(NameUnavailable());
            }
        }
    }

    public IEnumerator NameUnavailable()
    {
        unavailableText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        unavailableText.gameObject.SetActive(false);
    }
}