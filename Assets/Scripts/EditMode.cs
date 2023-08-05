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

    public GameObject editRay;
    public GameObject prefabToSpawn;

    public GameObject speakerTower;
    public GameObject subwoofer;
    public GameObject midrange;
    public GameObject tweeter;

    public GameObject ghostSpeakerTower;
    public GameObject ghostSubwoofer;
    public GameObject ghostMidrange;
    public GameObject ghostTweeter;

    private GameObject[] speakerTypes;
    private GameObject[] ghostSpeakerTypes;
    public GameObject[] selectedBGs;

    public int rayLength = 10;

    private int speakerIndex = 0;
    private bool timeout = false;

    private void Start()
    {
        editRay.SetActive(false);
        prefabToSpawn = speakerTower;
        speakerTypes = new GameObject[] {speakerTower, subwoofer, midrange, tweeter};
        ghostSpeakerTypes = new GameObject[] { ghostSpeakerTower, ghostSubwoofer, ghostMidrange, ghostTweeter };

        for (int i = 0; i < speakerTypes.Length; i++)
        {
            if (i != speakerIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }

        for (int i = 0; i < ghostSpeakerTypes.Length; i++)
        {
            ghostSpeakerTypes[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!editMenu.activeSelf) return;
        
        RaycastHit hit;

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
            {
                //// Draw ray and temporary speaker
                //GameObject temporaryRay = new GameObject();
                //temporaryRay.transform.position = transform.position;
                //temporaryRay.AddComponent<LineRenderer>();
                //LineRenderer lr = temporaryRay.GetComponent<LineRenderer>();
                //lr.startWidth = 0.01f;
                //lr.endWidth = 0.01f;
                //lr.SetPosition(0, temporaryRay.transform.position);
                //lr.SetPosition(1, hit.point);
                //lr.material = temporaryMat;

                //GameObject.Destroy(temporaryRay, 0.05f);
                // GameObject.Destroy(temporarySpeaker, 0.05f);

                // Create ghost speaker
                ghostSpeakerTypes[speakerIndex].SetActive(true);
                ghostSpeakerTypes[speakerIndex].transform.position = hit.point;

                Vector3 directionToTarget = transform.position - hit.point;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                ghostSpeakerTypes[speakerIndex].transform.rotation = targetRotation;
            }

            editRay.SetActive(true);
        }

        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * 10))
            {
                // Spawn speaker
                GameObject newObj = Instantiate(speakerTypes[speakerIndex], hit.point, Quaternion.identity);

                // Correct rotation
                Vector3 directionToTarget = transform.position - hit.point;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                newObj.transform.rotation = targetRotation;

                musicControls.CatchUpNewSpeaker(newObj.GetComponent<AudioSource>());
            }

            editRay.SetActive(false);
            ghostSpeakerTypes[speakerIndex].SetActive(false);
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

        // prefabToSpawn = speakerTypes[speakerIndex];
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