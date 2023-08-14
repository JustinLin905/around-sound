using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpeaker : MonoBehaviour
{
    public AudioClip pop;
    private int controllersInside = 0;

    private MusicControls musicControls;

    private void Start()
    {
        musicControls = GameObject.Find("EventSystem").GetComponent<MusicControls>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            controllersInside++;
            if (controllersInside >= 2)
            {
                StartCoroutine(DestroySpeaker());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            controllersInside--;
        }
    }

    private IEnumerator DestroySpeaker()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(pop);
        yield return new WaitForSeconds(0.1f);

        // If this is the last speaker, pause music
        if (GameObject.FindGameObjectsWithTag("Speakers").Length <= 1)
        {
            musicControls.nowPlaying = false;
            musicControls.playButton.sprite = musicControls.playButtonImage;
        }

        Destroy(gameObject);
    }
}
