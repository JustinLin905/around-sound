using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FrequencyFilter : MonoBehaviour
{
    AudioSource audioSource;
    AudioLowPassFilter lowPassFilter;
    AudioHighPassFilter highPassFilter;

    public int lowCutoff = 20;
    public int highCutoff = 250;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Add a low pass filter and set the cutoff frequency to 250 Hz
        // lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        // lowPassFilter.cutoffFrequency = highCutoff;

        // Add a high pass filter and set the cutoff frequency to 60 Hz
        // highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
        // highPassFilter.cutoffFrequency = lowCutoff;
    }
}
