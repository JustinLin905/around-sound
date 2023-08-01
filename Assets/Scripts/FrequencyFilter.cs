using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FrequencyFilter : MonoBehaviour
{
    AudioSource audioSource;
    AudioLowPassFilter lowPassFilter;
    AudioHighPassFilter highPassFilter;

    public int lowCutoff = 20;
    public int highCutoff = 250;
    public int cutoffSharpness = 3;
    public bool filterEnabled = true;

    void Start()
    {
        if (!filterEnabled)
        {
            return;
        }

        audioSource = GetComponent<AudioSource>();

        // Add a low pass filter and set the cutoff frequency to 250 Hz
        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = highCutoff;
        lowPassFilter.lowpassResonanceQ = cutoffSharpness; // Increase the sharpness of the filter

        // Add a high pass filter and set the cutoff frequency to 60 Hz
        highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
        highPassFilter.cutoffFrequency = lowCutoff;
        highPassFilter.highpassResonanceQ = cutoffSharpness; // Increase the sharpness of the filter
    }
}
