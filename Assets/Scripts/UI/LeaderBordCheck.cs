using TMPro;
using UnityEngine;

public class LeaderBordCheck : MonoBehaviour
{
    public ScoreUploadManager ScoreUploadManager;
    public TMP_Text ButtonText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreUploadManager.failed == true)
        {
            
            ButtonText.text = "failed";
        }
        else
        {
            ButtonText.text = "retry";
        }
        
    }
}
