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

    private GameObject activeMenu;
    private bool isAnimating = false;

    private void Start()
    {
        mainMenu.SetActive(true);
        editMenu.SetActive(false);
        musicControls.SetActive(false);
        environmentMenu.SetActive(false);

        activeMenu = mainMenu;
    }
    
    private void Update()
    {
        if (isAnimating) return;

        if (OVRInput.GetDown(OVRInput.Button.Four) && mainMenu.activeSelf)
        {
            StartCoroutine(SetMenuActive(editMenu));
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (mainMenu.activeSelf)
            {
                StartCoroutine(SetMenuActive(mainMenu, true));
            }
            else
            {
                StartCoroutine(SetMenuActive(mainMenu));
            }
        }

        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            StartCoroutine(SetMenuActive(musicControls));

        }
        
        else if (OVRInput.GetDown(OVRInput.Button.Three) && mainMenu.activeSelf) {
            StartCoroutine(SetMenuActive(environmentMenu));
        }
    }

    private IEnumerator SetMenuActive(GameObject menu, bool disableAll = false)
    {
        isAnimating = true;

        if (activeMenu.activeSelf)
        {
            anim = activeMenu.GetComponent<Animator>();
            anim.Play("Base Layer.Shrinking");
            yield return new WaitForSeconds(0.3f);
            activeMenu.SetActive(false);
        }

        if (!disableAll)
        {
            menu.SetActive(true);
            anim = menu.GetComponent<Animator>();
            anim.Play("Base Layer.Growing");
            activeMenu = menu;
            yield return new WaitForSeconds(0.3f);
        }
        
        isAnimating = false;
    }
}