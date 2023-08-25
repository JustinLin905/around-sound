using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private bool started = false;
    public GameObject player;
    public GameObject apartmentSpawnAnchor;
    public GameObject[] orbs;

    public ChangeMenus changeMenus;
    public FogController fogController;

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
        // Roll dark fog
        fogController.ShowFog(0.8f);
        
        foreach (GameObject orb in orbs)
        {
            Destroy(orb);
        }
        yield return new WaitForSeconds(2f);

        // Teleport player to apartment
        OVRPlayerController playerController = player.GetComponent<OVRPlayerController>();
        playerController.enabled = false;
        player.transform.position = apartmentSpawnAnchor.transform.position;
        
        yield return new WaitForSeconds(0.05f);
        playerController.enabled = true;
        changeMenus.ViewMainMenu();
    }

}
