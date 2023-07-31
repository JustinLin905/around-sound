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

    private Animator anim;

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
            StartCoroutine(SetMenuActive(mainMenu, false));
            StartCoroutine(SetMenuActive(editMenu, true));
            StartCoroutine(SetMenuActive(musicControls, false));
            StartCoroutine(SetMenuActive(environmentMenu, false));
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            StartCoroutine(SetMenuActive(editMenu, false));
            StartCoroutine(SetMenuActive(musicControls, false));
            StartCoroutine(SetMenuActive(environmentMenu, false));

            if (mainMenu.activeSelf)
            {
                StartCoroutine(SetMenuActive(mainMenu, false));
            }
            else
            {
                StartCoroutine(SetMenuActive(mainMenu, true));
            }
        }

        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            StartCoroutine(SetMenuActive(mainMenu, false));
            StartCoroutine(SetMenuActive(editMenu, false));
            StartCoroutine(SetMenuActive(musicControls, true));
            StartCoroutine(SetMenuActive(environmentMenu, false));

        }
        
        else if (OVRInput.GetDown(OVRInput.Button.Three) && mainMenu.activeSelf) {
            StartCoroutine(SetMenuActive(mainMenu, false));
            StartCoroutine(SetMenuActive(editMenu, false));
            StartCoroutine(SetMenuActive(musicControls, false));
            StartCoroutine(SetMenuActive(environmentMenu, true));
        }
    }

    private IEnumerator SetMenuActive(GameObject menu, bool active)
    {
        if (active) {
            menu.SetActive(true);
            anim = menu.GetComponent<Animator>();
            anim.Play("Base Layer.Growing");
        }

        else {
            anim = menu.GetComponent<Animator>();
            anim.Play("Base Layer.Shrinking");
            yield return new WaitForSeconds(0.2f);
            menu.SetActive(false);
        }
    }
}