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
    public int defaultLowCutoff = 20;
    public int defaultHighCutoff = 250;
    public int defaultCutoffSharpness = 3;
    public bool filterEnabled = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (!filterEnabled) return;

        // Add a low & high pass filter
        if (lowPassFilter == null)
        {
            lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        }
        
        if (highPassFilter == null)
        {
            highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
        }

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

                    if (lowCutoff > highCutoff)
                    {
                        highCutoff = lowCutoff;
                    }
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

                    if (highCutoff < lowCutoff)
                    {
                        lowCutoff = highCutoff;
                    }
                    break;
                case 2:
                    cutoffSharpness -= 1;
                    if (cutoffSharpness < 1)
                    {
                        cutoffSharpness = 1;
                    }
                    break;
            }

            if (lowCutoff < 0) 
            {
                lowCutoff = 0;
            }

            if (highCutoff < 0)
            {
                highCutoff = 0;
            }
        }

        UpdateFilters();
    }

    public void ResetFrequencies()
    {
        lowCutoff = defaultLowCutoff;
        highCutoff = defaultHighCutoff;
        cutoffSharpness = defaultCutoffSharpness;

        UpdateFilters();
    }

    private void UpdateFilters()
    {
        if (lowPassFilter == null)
        {
            lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        }

        if (highPassFilter == null)
        {
            highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();
        }

        if (filterEnabled)
        {
            lowPassFilter.cutoffFrequency = highCutoff;
            lowPassFilter.lowpassResonanceQ = cutoffSharpness;

            highPassFilter.cutoffFrequency = lowCutoff;
            highPassFilter.highpassResonanceQ = cutoffSharpness;
        }
    }
}