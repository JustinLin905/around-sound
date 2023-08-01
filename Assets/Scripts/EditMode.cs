using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class EditMode : MonoBehaviour
{
    public MusicControls musicControls;
    
    public GameObject editMenu;
    public Material temporaryMat;

    public GameObject prefabToSpawn;

    public GameObject speakerTower;
    public GameObject subwoofer;
    public GameObject midrange;
    public GameObject tweeter;
    private GameObject[] speakerTypes;
    public GameObject[] selectedBGs;

    public int rayLength = 10;

    private int speakerIndex = 0;
    private bool timeout = false;

    private void Start()
    {
        prefabToSpawn = speakerTower;
        speakerTypes = new GameObject[] {speakerTower, subwoofer, midrange, tweeter};
    }

    void Update()
    {
        if (!editMenu.activeSelf) return;
        
        RaycastHit hit;

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
            {
                // Draw ray and temporary speaker
                GameObject temporaryRay = new GameObject();
                temporaryRay.transform.position = transform.position;
                temporaryRay.AddComponent<LineRenderer>();
                LineRenderer lr = temporaryRay.GetComponent<LineRenderer>();
                lr.startWidth = 0.01f;
                lr.endWidth = 0.01f;
                lr.SetPosition(0, temporaryRay.transform.position);
                lr.SetPosition(1, hit.point);
                lr.material = temporaryMat;
                
                GameObject.Destroy(temporaryRay, 0.05f);
                // GameObject.Destroy(temporarySpeaker, 0.05f);
            }
        }
        
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
            {
                // Spawn speaker
                GameObject newObj = Instantiate(prefabToSpawn, hit.point, Quaternion.identity);
                musicControls.CatchUpNewSpeaker(newObj.GetComponent<AudioSource>());
            }
        }

        if (OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");
            foreach (GameObject speaker in speakers)
            {
                Destroy(speaker);
            }

            musicControls.PushButton(musicControls.playButton);
            musicControls.nowPlaying = false;
        }

        // Choose speaker type
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.5f && !timeout)
        {
            SwapSpeakerType(false);
        }
        else if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.5f && !timeout)
        {
            SwapSpeakerType(true);
        }
    }

    void SwapSpeakerType(bool down)
    {
        StartCoroutine(SetTimeout());

        if (down)
        {
            speakerIndex++;
            if (speakerIndex >= speakerTypes.Length)
            {
                speakerIndex = 0;
            }
        }
        else
        {
            speakerIndex--;
            if (speakerIndex < 0)
            {
                speakerIndex = speakerTypes.Length - 1;
            }
        }

        prefabToSpawn = speakerTypes[speakerIndex];
        selectedBGs[speakerIndex].SetActive(true);

        for (int i = 0; i < speakerTypes.Length; i++)
        {
            if (i != speakerIndex)
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