using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text audiotext;
    int min = 0, max = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.minValue = min;
        slider.maxValue = max;
        AudioListener.volume = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changevolume()
    {
        AudioListener.volume = slider.value;
        audiotext.text = slider.value.ToString();
    }
}
