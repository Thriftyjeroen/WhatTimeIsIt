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
    [SerializeField] TMP_Text errorText;

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
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(DisplayError("No internet connection, please connect to the internet to continue"));
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequest.Get($"https://thriftyjeroen.nl/api/score/playerAvailable?name={nameInput.text}"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(DisplayError("Server error, please try again later"));
                yield break;
            }

            bool available = System.Convert.ToBoolean(www.downloadHandler.text);

            if (available)
            {
                PlayerPrefs.SetString("Name", nameInput.text);
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                StartCoroutine(DisplayError("Name Unavailable"));
            }
        }
    }

    public IEnumerator DisplayError(string error)
    {
        errorText.text = error;
        errorText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        errorText.gameObject.SetActive(false);
    }
}