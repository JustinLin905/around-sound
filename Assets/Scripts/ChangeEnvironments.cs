using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironments : MonoBehaviour
{
    public GameObject[] environments;
    public GameObject[] selectedBGs;

    private int environmentIndex = 0;
    private bool timeout = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < environments.Length; i++)
        {
            if (i != environmentIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.5f && !timeout)
        {
            SelectEnvironment(false);
        }
        else if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.5f && !timeout)
        {
            SelectEnvironment(true);
        }
    }

    void SelectEnvironment(bool down)
    {
        StartCoroutine(SetTimeout());

        if (down)
        {
            environmentIndex++;
            if (environmentIndex >= environments.Length)
            {
                environmentIndex = 0;
            }
        }
        else
        {
            environmentIndex--;
            if (environmentIndex < 0)
            {
                environmentIndex = environments.Length - 1;
            }
        }

        selectedBGs[environmentIndex].SetActive(true);

        for (int i = 0; i < environments.Length; i++)
        {
            if (i != environmentIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }
    }

    IEnumerator SetTimeout()
    {
        timeout = true;
        yield return new WaitForSeconds(0.15f);
        timeout = false;
    }
}
