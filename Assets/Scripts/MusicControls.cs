using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MusicControls : MonoBehaviour
{
    public AudioClip clip; // The AudioClip you want to play

    public GameObject errorText;
    
    private AudioClip currentSong;
    private float currentTime;
    public bool nowPlaying = false;
    private AudioSource activeAudiosource;
    private bool timeout = false;

    private void Start()
    {
        errorText.SetActive(false);
        currentSong = clip;
        currentTime = 0f;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            PlayAudio();
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            RewindAudio();
        }

        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            // SkipAudio();
        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.5f && !timeout)
        {
            StartCoroutine(SeekTenSeconds(true));
        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.5f && !timeout)
        {
            StartCoroutine(SeekTenSeconds(false));
        }


        UpdateMetadata();
    }

    void PlayAudio()
    {
        // Find all GameObjects with the tag "Speakers"
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        if (speakers.Length == 0)
        {
            errorText.SetActive(true);
            return;
        }

        errorText.SetActive(false);

        // If any audio source is playing, pause all audio sources
        if (nowPlaying)
        {
            foreach (GameObject speaker in speakers)
            {
                AudioSource audioSource = speaker.GetComponent<AudioSource>();
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }

            nowPlaying = false;
        }
        // Otherwise, play the audio clip on all audio sources
        else
        {
            foreach (GameObject speaker in speakers)
            {
                AudioSource audioSource = speaker.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    // Assign the clip to the AudioSource and play it
                    audioSource.clip = clip;
                    if (activeAudiosource)
                    {
                        audioSource.time = activeAudiosource.time;

                    }
                    
                    audioSource.Play();
                    activeAudiosource = audioSource;
                }
            }

            nowPlaying = true;
        }
    }

    void RewindAudio()
    {
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        foreach (GameObject speaker in speakers)
        {
            AudioSource audioSource = speaker.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                // audioSource.clip = clip;
                audioSource.time = 0f;
            }
        }
    }

    void UpdateMetadata()
    {
        // currentTime = activeAudiosource.time;
    }

    // Function to handle new speaker being created while music is already playing
    public void CatchUpNewSpeaker(AudioSource newSpeaker)
    {
        if (nowPlaying)
        {
            newSpeaker.clip = clip;
            newSpeaker.time = activeAudiosource.time;
            newSpeaker.Play();
        }
    }

    private IEnumerator SeekTenSeconds(bool reverse)
    {
        timeout = true;

        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        foreach (GameObject speaker in speakers)
        {
            AudioSource audioSource = speaker.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = clip;

                if (reverse)
                {
                    audioSource.time -= 10f;
                }

                else
                {
                    audioSource.time += 10f;
                }
            }
        }

        yield return new WaitForSeconds(1f);
        timeout = false;
    }
}
