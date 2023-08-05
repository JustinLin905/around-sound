using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MusicControls : MonoBehaviour
{
    public AudioClip clip; // The AudioClip you want to play
    public GameObject musicControlsMenu;

    public GameObject errorText;
    public TextMeshProUGUI songTimeText;
    public TextMeshProUGUI songTitleText;

    private AudioClip currentSong;
    private float currentTime;
    public bool nowPlaying;
    private AudioSource activeAudiosource;

    // Images
    public Image playButton;
    public Image rewindButton;
    public Image forwardButton;
    public Image seekBackButton;
    public Image seekForwardButton;
    public Sprite playButtonImage;
    public Sprite pauseButtonImage;

    private void Start()
    {
        errorText.SetActive(false);
        currentSong = clip;
        currentTime = 0f;
        nowPlaying = false;
    }

    private void OnEnable()
    {
        errorText.SetActive(false);
    }

    private void Update()
    {
        if (!musicControlsMenu.activeSelf) return;

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
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            SeekTenSeconds(true);
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            SeekTenSeconds(false);
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
        StartCoroutine(PushButtonVisual(playButton));

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
                // Add conditional for rewind or last track
                // audioSource.clip = clip;
                audioSource.time = 0f;
            }
        }

        StartCoroutine(PushButtonVisual(rewindButton));
    }

    void UpdateMetadata()
    { 
        if (activeAudiosource != null && activeAudiosource.clip != null)
        {
            currentTime = activeAudiosource.time;
            float totalTime = activeAudiosource.clip.length;

            string currentMinutes = Mathf.Floor(currentTime / 60).ToString("00");
            string currentSeconds = Mathf.Floor(currentTime % 60).ToString("00");

            string totalMinutes = Mathf.Floor(totalTime / 60).ToString("00");
            string totalSeconds = Mathf.Floor(totalTime % 60).ToString("00");

            songTimeText.text = currentMinutes + ":" + currentSeconds + " / " + totalMinutes + ":" + totalSeconds;
            songTitleText.text = activeAudiosource.clip.name;
        }
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

    void SeekTenSeconds(bool reverse)
    {
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        foreach (GameObject speaker in speakers)
        {
            AudioSource audioSource = speaker.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = clip;

                if (reverse)
                {
                    if (audioSource.time < 10f)
                    {
                        audioSource.time = 0f;
                    }
                    else
                    {
                        audioSource.time -= 10f;
                    }
                    
                    StartCoroutine(PushButtonVisual(seekBackButton));
                }

                else
                {
                    audioSource.time += 10f;
                    StartCoroutine(PushButtonVisual(seekForwardButton));
                }
            }
        }
    }

    IEnumerator PushButtonVisual(Image buttonImage)
    {
        if (buttonImage == playButton)
        {
            if (nowPlaying)
            {
                buttonImage.sprite = playButtonImage;
            }
            else
            {
                buttonImage.sprite = pauseButtonImage;
            }
        }

        buttonImage.CrossFadeAlpha(0.1f, 0, true);
        yield return new WaitForSeconds(0.1f);
        buttonImage.CrossFadeAlpha(1f, 1f, true);
    }

    public void PushButton(Image buttonImage) 
    {
        StartCoroutine(PushButtonVisual(buttonImage));
    }
}
