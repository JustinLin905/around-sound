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
    public GameObject editSpeakerMenu;

    private Animator anim;

    private GameObject activeMenu;
    private bool isAnimating = false;
    private bool menuHidden = false;

    public EditMode editMode;

    private void Start()
    {
        mainMenu.SetActive(false);
        editMenu.SetActive(false);
        musicControls.SetActive(false);
        environmentMenu.SetActive(false);
        editSpeakerMenu.SetActive(false);

        activeMenu = mainMenu;
    }
    
    private void Update()
    {
        if (isAnimating) return;

        if (OVRInput.GetDown(OVRInput.Button.Four) && mainMenu.activeSelf)
        {
            StartCoroutine(SetMenuActive(editMenu));
        }

        else if (OVRInput.GetDown(OVRInput.Button.Two) && !(editMenu.activeSelf && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)))
        {
            if (mainMenu.activeSelf)
            {
                StartCoroutine(SetMenuActive(mainMenu, true));
            }
            else if (editSpeakerMenu.activeSelf) {
                StartCoroutine(SetMenuActive(editMenu));
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

        else if (OVRInput.GetDown(OVRInput.Button.One) && editMenu.activeSelf && editMode.IsEditableSpeaker())
        {
            StartCoroutine(SetMenuActive(editSpeakerMenu));
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
            menuHidden = false;
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            menuHidden = true;
        }
        
        isAnimating = false;
    }

    public void ViewMainMenu()
    {
        StartCoroutine(SetMenuActive(mainMenu));
    }

    public bool IsMenuHidden()
    {
        return menuHidden;
    }
}