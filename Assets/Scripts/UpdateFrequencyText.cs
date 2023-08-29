using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script is to keep the frequency ranges displayed in the Edit Mode UI up to date
public class UpdateFrequencyText : MonoBehaviour
{
    public TextMeshProUGUI subwooferRange;
    public TextMeshProUGUI midrangeRange;
    public TextMeshProUGUI tweeterRange;

    public FrequencyFilter subwooferFilter;
    public FrequencyFilter midrangeFilter;
    public FrequencyFilter tweeterFilter;

    void OnEnable()
    {
        subwooferRange.text = subwooferFilter.lowCutoff.ToString() + " - " + subwooferFilter.highCutoff.ToString() + " Hz";
        midrangeRange.text = midrangeFilter.lowCutoff.ToString() + " - " + midrangeFilter.highCutoff.ToString() + " Hz";
        tweeterRange.text = tweeterFilter.lowCutoff.ToString() + " - " + tweeterFilter.highCutoff.ToString() + " Hz";
    }
}
