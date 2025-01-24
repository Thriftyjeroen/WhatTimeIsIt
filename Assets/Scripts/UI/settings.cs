using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    [SerializeField] Slider slider; // Slider for volume control
    [SerializeField] TMP_Text audiotext; // Text to display the volume value
    int min = 0, max = 100; // Minimum and maximum values for the slider

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.minValue = min; // Set slider minimum value
        slider.maxValue = max; // Set slider maximum value
        AudioListener.volume = 1.0f; // Initialize audio volume to full
    }

    // Update is called once per frame
    void Update()
    {
        // Not used in this script
    }

    public void changevolume()
    {
        // Set slider value to match the current volume level
        slider.value = AudioListener.volume;

        // Add listener to respond to slider value changes
        slider.onValueChanged.AddListener(onvolumechange);

        // Update the volume text display
        audiotext.text = slider.value.ToString();
    }

    void onvolumechange(float value)
    {
        // Update the audio volume based on the slider value
        AudioListener.volume = value;
    }
}
