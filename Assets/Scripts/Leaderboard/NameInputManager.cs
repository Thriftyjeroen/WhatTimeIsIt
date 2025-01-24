using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NameInputManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput; // Input field for the player's name
    [SerializeField] TMP_Text errorText; // Text object for displaying errors

    // Start is called before the first frame update
    public void Start()
    {
        // If player name is already saved, load the main menu
        if (PlayerPrefs.HasKey("Name"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        nameInput.characterLimit = 15; // Set character limit for the name input
    }

    // Start the crusade when player submits their name
    public void StartCrusadeStarter()
    {
        StartCoroutine(StartCrusade()); // Start the coroutine for checking name availability
    }

    // Coroutine to check if the player's name is available
    public IEnumerator StartCrusade()
    {
        // Check if there is an internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(DisplayError("No internet connection, please connect to the internet to continue"));
            yield break; // Exit the coroutine if no internet connection
        }

        // Send a request to the server to check if the name is available
        using (UnityWebRequest www = UnityWebRequest.Get($"https://thriftyjeroen.nl/api/score/playerAvailable?name={nameInput.text}"))
        {
            yield return www.SendWebRequest();

            // If there's a server error, display an error message
            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(DisplayError("Server error, please try again later"));
                yield break;
            }

            // Parse the response to see if the name is available
            bool available = System.Convert.ToBoolean(www.downloadHandler.text);

            if (available)
            {
                PlayerPrefs.SetString("Name", nameInput.text); // Save the player's name
                SceneManager.LoadScene("MainMenu"); // Load the main menu
            }
            else
            {
                StartCoroutine(DisplayError("Name Unavailable")); // Show error if name is taken
            }
        }
    }

    // Coroutine to display an error message for a short time
    public IEnumerator DisplayError(string error)
    {
        errorText.text = error; // Set the error message
        errorText.gameObject.SetActive(true); // Show the error text
        yield return new WaitForSecondsRealtime(2); // Wait for 2 seconds
        errorText.gameObject.SetActive(false); // Hide the error text
    }
}
