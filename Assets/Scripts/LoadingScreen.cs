using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private bool started = false;
    public GameObject player;
    public GameObject apartmentSpawnAnchor;

    public ChangeMenus changeMenus;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && !started)
        {
            started = true;
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        OVRPlayerController playerController = player.GetComponent<OVRPlayerController>();
        playerController.enabled = false;
        player.transform.position = apartmentSpawnAnchor.transform.position;
        playerController.enabled = true;
        Debug.Log("Teleporting player to: " + player.transform.position);
        yield return new WaitForSeconds(0.01f);
        changeMenus.ViewMainMenu();
    }

}
