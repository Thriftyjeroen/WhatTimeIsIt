using System;
using TMPro;
using UnityEngine;

public class MultIncreaseManager : MonoBehaviour
{
    TMP_Text score;
    AudioSource source;
    private void Start()
    {
        score = GameObject.FindGameObjectsWithTag("TempScore")[0].GetComponent<TMP_Text>();
        source = GetComponent<AudioSource>();
        source.pitch = 1 + (Int32.Parse(score.text.Split("x ")[1]) * 0.05946f);
    }
    void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
