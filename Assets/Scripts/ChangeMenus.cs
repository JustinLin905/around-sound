using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMenus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject editMenu;
    public GameObject musicControls;

    private void Start()
    {
        mainMenu.SetActive(true);
        editMenu.SetActive(false);
        musicControls.SetActive(false);
    }
    
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four) && !musicControls.activeSelf)
        {
             mainMenu.SetActive(false);
             editMenu.SetActive(true);
            musicControls.SetActive(false);
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            mainMenu.SetActive(true);
            editMenu.SetActive(false);
            musicControls.SetActive(false);
            
        }

        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            mainMenu.SetActive(false);
            editMenu.SetActive(false);
            musicControls.SetActive(true);
        }
    }
}