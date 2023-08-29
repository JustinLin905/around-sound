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

        // Gonna have to change to InChildren
        audioSource = GetComponent<AudioSource>();

        // Add a low & high pass filter
        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();

        UpdateFilters();
    }

    public void EditFrequency(int selectionIndex, bool down = false)
    {
        if (!filterEnabled)
        {
            return;
        }

        if (!down)
        {
            // Increase frequency
            switch (selectionIndex)
            {
                case 0:
                    lowCutoff += 5;
                    break;
                case 1:
                    highCutoff += 5;
                    break;
                case 2:
                    cutoffSharpness += 1;
                    break;
            }
        }
        else
        {
            // Decrease frequency
            switch (selectionIndex)
            {
                case 0:
                    lowCutoff -= 5;
                    break;
                case 1:
                    highCutoff -= 5;
                    break;
                case 2:
                    cutoffSharpness -= 1;
                    if (cutoffSharpness < 1)
                    {
                        cutoffSharpness = 1;
                    }
                    break;
            }
        }

        UpdateFilters();
    }

    private void UpdateFilters()
    {
        lowPassFilter.cutoffFrequency = highCutoff;
        lowPassFilter.lowpassResonanceQ = cutoffSharpness;

        highPassFilter.cutoffFrequency = lowCutoff;
        highPassFilter.highpassResonanceQ = cutoffSharpness;
    }
}
