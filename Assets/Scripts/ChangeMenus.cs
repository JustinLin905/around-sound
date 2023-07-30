using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMenus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject editMenu;
    public GameObject musicControls;
    public GameObject environmentMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        editMenu.SetActive(false);
        musicControls.SetActive(false);
        environmentMenu.SetActive(false);
    }
    
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four) && mainMenu.activeSelf)
        {
             mainMenu.SetActive(false);
             editMenu.SetActive(true);
             musicControls.SetActive(false);
             environmentMenu.SetActive(false);
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            editMenu.SetActive(false);
            musicControls.SetActive(false);
            environmentMenu.SetActive(false);

            if (musicControls.activeSelf)
            {
                mainMenu.SetActive(false);
            }
            else
            {
                mainMenu.SetActive(true);
            }
        }

        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            mainMenu.SetActive(false);
            editMenu.SetActive(false);
            musicControls.SetActive(true);
            environmentMenu.SetActive(false);
        }
        
        else if (OVRInput.GetDown(OVRInput.Button.Three) && mainMenu.activeSelf) {
            mainMenu.SetActive(false);
            editMenu.SetActive(false);
            musicControls.SetActive(false);
            environmentMenu.SetActive(true);
        }
    }
}