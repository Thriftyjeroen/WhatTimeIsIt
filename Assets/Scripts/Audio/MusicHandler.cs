using TMPro;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] TMP_Text tempScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    AudioSource[] stems;
    void Awake()
    {
        stems = GetComponents<AudioSource>();
        stems[2].volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempScore.text != "0")
        {
            stems[1].volume = 0;
            stems[2].volume = 1;
        }
        else
        {
            stems[1].volume = 1;
            stems[2].volume = 0;
        }
    }
}
