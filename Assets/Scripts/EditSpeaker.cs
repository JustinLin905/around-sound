using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditSpeaker : MonoBehaviour
{
    private GameObject speakerPrefab;
    private FrequencyFilter frequencyFilter;
    public GameObject[] selectedBGs;

    public TextMeshProUGUI speakerTypeText;
    public TextMeshProUGUI lowCutoffText;
    public TextMeshProUGUI highCutoffText;
    public TextMeshProUGUI sharpnessText;

    private int selectionIndex = 0;     // Used to choose what option to edit (High Cutoff, Low Cutoff, or Sharpness)
    private bool timeout = false;

    public EditMode editMode;

    void Start()
    {
        for (int i = 0; i < selectedBGs.Length; i++)
        {
            if (i != selectionIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        speakerPrefab = editMode.GetSpeakerType();
        frequencyFilter = speakerPrefab.GetComponent<FrequencyFilter>();
        speakerTypeText.text = "Edit" + speakerPrefab.name;
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.5f && !timeout)
        {
            SwapSelection(false);
        }
        else if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.5f && !timeout)
        {
            SwapSelection(true);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !timeout)
        {
            frequencyFilter.EditFrequency(selectionIndex, true);
            StartCoroutine(SetTimeout());
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !timeout)
        {
            frequencyFilter.EditFrequency(selectionIndex, false);
            StartCoroutine(SetTimeout());
        }

        UpdateText();
    }

    void SwapSelection(bool down)
    {
        StartCoroutine(SetTimeout());

        if (down)
        {
            selectionIndex++;
            if (selectionIndex >= 3)
            {
                selectionIndex = 0;
            }
        }
        else
        {
            selectionIndex--;
            if (selectionIndex < 0)
            {
                selectionIndex = 2;
            }
        }

        selectedBGs[selectionIndex].SetActive(true);

        for (int i = 0; i < selectedBGs.Length; i++)
        {
            if (i != selectionIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }
    }

    void UpdateText()
    {
        lowCutoffText.text = frequencyFilter.lowCutoff.ToString() + " Hz";
        highCutoffText.text = frequencyFilter.highCutoff.ToString() + " Hz";
        sharpnessText.text = frequencyFilter.cutoffSharpness.ToString();
    }

    IEnumerator SetTimeout()
    {
        timeout = true;
        yield return new WaitForSeconds(0.15f);
        timeout = false;
    }
}
