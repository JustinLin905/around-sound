using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironments : MonoBehaviour
{
    public GameObject player;
    public FogController fogController;

    public GameObject[] environments;       // Teleport anchors for each environment
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

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartCoroutine(ChangeEnvironment());
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

    IEnumerator ChangeEnvironment()
    {
        // Roll dark fog
        fogController.ShowFog(Constants.TELEPORT_FOG_DENSITY);
        yield return new WaitForSeconds(2f);

        // Teleport player to selected spawn anchor
        OVRPlayerController playerController = player.GetComponent<OVRPlayerController>();
        playerController.enabled = false;
        player.transform.position = environments[environmentIndex].transform.position;
        
        yield return new WaitForSeconds(0.05f);
        playerController.enabled = true;
    }

    IEnumerator SetTimeout()
    {
        timeout = true;
        yield return new WaitForSeconds(0.15f);
        timeout = false;
    }
}
