using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
        // Ensure the slider value is set to the current volume level when the game starts
        slider.value = AudioListener.volume;

        // Add listener to handle slider changes
        slider.onValueChanged.AddListener(onvolumechange);
        audiotext.text = slider.value.ToString();   
    }
    void onvolumechange(float value)
    {
        AudioListener.volume = value;
    }
}
