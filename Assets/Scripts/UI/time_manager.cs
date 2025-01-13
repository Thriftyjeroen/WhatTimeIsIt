using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class time : MonoBehaviour
{
    [SerializeField] TMP_Text TimeText;
    int minutes, seconds;
    [SerializeField] float SpendTime;

    private void Start()
    {
        GameObject Object = GameObject.Find("Time");
        if (Object != null)
        {
            TimeText = Object.GetComponent<TMP_Text>();
        }
        else
        {

        }
    }
   
    void OnEnable()
    {
        
        SceneManager.sceneLoaded += ResetTime;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= ResetTime;
    }
    // Update is called once per frame
    void Update()
    {
        SpendTime += Time.deltaTime;
        seconds = Mathf.FloorToInt(SpendTime % 60);
        minutes = Mathf.FloorToInt(SpendTime / 60);
        settext();
    }
    void settext()
    {
        TimeText.text = $"Time {minutes}:{seconds:D2}";
    }

    void ResetTime(Scene scene, LoadSceneMode mode)
    {
        checktext();
        SpendTime = 0f;
        seconds = 0;
        minutes = 0;
    }
    void checktext()
    {
        GameObject Object = GameObject.Find("Time");
        if (Object != null)
        {
            TimeText = Object.GetComponent<TMP_Text>();
        }
        else
        {

        }
    }
}
