using TMPro;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] TMP_Text tempScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    AudioSource[] stems;
    void Awake()
    {
        // Gets the AudioSource component of this gameobject
        stems = GetComponents<AudioSource>();
        stems[2].volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the temp score is 0
        if (tempScore.text != "0")
        {
            // Sets volume of the stems
            stems[1].volume = 0;
            stems[2].volume = 0.5f;
        }
        else
        {
            stems[1].volume = 1;
            stems[2].volume = 0;
        }
    }
}
