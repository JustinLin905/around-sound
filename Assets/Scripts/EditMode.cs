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

    public GameObject rightHandAnchor;
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

    public AudioSource universalAudiosource;
    public AudioClip deleteSound;

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
            if (Physics.Raycast(rightHandAnchor.transform.position, rightHandAnchor.transform.forward, out hit, rayLength * 10))
            {
                // Create ghost speaker
                ghostSpeakerTypes[speakerIndex].SetActive(true);
                ghostSpeakerTypes[speakerIndex].transform.position = hit.point;

                Vector3 directionToTarget = rightHandAnchor.transform.position - hit.point;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                ghostSpeakerTypes[speakerIndex].transform.rotation = targetRotation;

                // Disable other ghost speakers
                for (int i = 0; i < ghostSpeakerTypes.Length; i++)
                {
                    if (i != speakerIndex)
                    {
                        ghostSpeakerTypes[i].SetActive(false);
                    }
                }

                editRay.SetActive(true);
            }
        }

        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(rightHandAnchor.transform.position, rightHandAnchor.transform.forward, out hit, rayLength * 10))
            {
                // Spawn speaker
                GameObject newObj = Instantiate(speakerTypes[speakerIndex], hit.point, Quaternion.identity);

                // Correct rotation
                Vector3 directionToTarget = rightHandAnchor.transform.position - hit.point;
                directionToTarget.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                newObj.transform.rotation = targetRotation;

                musicControls.CatchUpNewSpeaker(newObj.GetComponent<AudioSource>());
            }

            editRay.SetActive(false);
            
            // Disable all ghost speakers
            for (int i = 0; i < ghostSpeakerTypes.Length; i++)
            {
                ghostSpeakerTypes[i].SetActive(false);
            }
        }

        // Destroy all speakers
        if (OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            DestroyAllSpeakers();
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

        selectedBGs[speakerIndex].SetActive(true);

        for (int i = 0; i < speakerTypes.Length; i++)
        {
            if (i != speakerIndex)
            {
                selectedBGs[i].SetActive(false);
            }
        }
    }

    public void DestroyAllSpeakers()
    {
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");
        foreach (GameObject speaker in speakers)
        {
            Destroy(speaker);
        }

        // Disable all ghost speakers
        for (int i = 0; i < ghostSpeakerTypes.Length; i++)
        {
            ghostSpeakerTypes[i].SetActive(false);
        }

        musicControls.nowPlaying = false;
        musicControls.playButton.sprite = musicControls.playButtonImage;
        universalAudiosource.PlayOneShot(deleteSound);
    }

    public GameObject GetSpeakerType()
    {
        return speakerTypes[speakerIndex];
    }

    public bool IsEditableSpeaker()
    {
        return speakerIndex != 0;
    }

    IEnumerator SetTimeout()
    {
        timeout = true;
        yield return new WaitForSeconds(0.15f);
        timeout = false;
    }
}