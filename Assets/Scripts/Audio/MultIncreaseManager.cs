using System;
using TMPro;
using UnityEngine;

public class MultIncreaseManager : MonoBehaviour
{
    private ScoreManager scoreManager;
    TMP_Text score;
    AudioSource source;
    private void Start()
    {
        scoreManager = GameObject.FindGameObjectsWithTag("scoreManager")[0].GetComponent<ScoreManager>();
        source = GetComponent<AudioSource>();
        source.pitch = 1 + (scoreManager.GetMult() * 0.05946f);
    }
    void Update()
    {
        if (source != null && !source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
