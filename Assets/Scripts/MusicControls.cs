using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MusicControls : MonoBehaviour
{
    public AudioClip clip; // The AudioClip you want to play
    private Song currentSong;
    public GameObject musicControlsMenu;

    public GameObject errorText;
    public TextMeshProUGUI songTimeText;
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI nextUpText;
    public TextMeshProUGUI mainQueueText;

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

    // For music queue
    private List<Song> queue = new List<Song>();
    private int currentSongIndex = 0;
    private const int MAX_DISPLAYED_SONGS = 3;
    private List<TextMeshProUGUI> queueTexts;

    private void Start()
    {
        errorText.SetActive(false);
        currentTime = 0f;
        nowPlaying = false;

        // Get all TextMeshProUGUI with tag "Queue Text". This is to update all canvases which display the queue
        queueTexts = new List<TextMeshProUGUI>();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Queue Text")) 
        {
            queueTexts.AddRange(go.GetComponentsInChildren<TextMeshProUGUI>());
            Debug.Log("Foundqueuetext");
        }

        // Add handheld queue text to list, since it will be inactive at start and will not be found by the above
        queueTexts.Add(mainQueueText);
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
            PlayPause();
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            RewindAudio();
        }

        if (OVRInput.GetDown(OVRInput.Button.Four) || (activeAudiosource != null && activeAudiosource.time >= activeAudiosource.clip.length))
        {
            SkipAudio();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SeekTenSeconds(true);
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            SeekTenSeconds(false);
        }

        UpdateMetadata();
    }

    void PlayPause()
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
                    audioSource.clip = currentSong.clip;
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
        if (activeAudiosource != null && activeAudiosource.time < 2f)
        {
            currentSongIndex--;

            if (currentSongIndex < 0)
            {
                currentSongIndex = queue.Count - 1;
            }

            // Set the new song
            currentSong = queue[currentSongIndex];
            clip = currentSong.clip;
        }

        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        foreach (GameObject speaker in speakers)
        {
            AudioSource audioSource = speaker.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.time = 0f;
                audioSource.Play();
                activeAudiosource = audioSource;
            }
        }

        StartCoroutine(PushButtonVisual(rewindButton));
        playButton.sprite = pauseButtonImage;
        nowPlaying = true;
        SetQueueText();
    }

    void SkipAudio()
    {
        currentSongIndex++;
        if (currentSongIndex >= queue.Count)
        {
            currentSongIndex = 0;
        }

        currentSong = queue[currentSongIndex];
        clip = currentSong.clip;

        // Play the new song
        GameObject[] speakers = GameObject.FindGameObjectsWithTag("Speakers");

        foreach (GameObject speaker in speakers)
        {
            AudioSource audioSource = speaker.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.time = 0f;
                audioSource.Play();
                activeAudiosource = audioSource;
            }
        }

        StartCoroutine(PushButtonVisual(forwardButton));
        playButton.sprite = pauseButtonImage;
        nowPlaying = true;
        SetQueueText();
    }

    void UpdateMetadata()
    { 
        // Second conditional may be unnecessary
        if (activeAudiosource != null && activeAudiosource.clip != null)
        {
            currentTime = activeAudiosource.time;
            float totalTime = currentSong.clip.length;

            string currentMinutes = Mathf.Floor(currentTime / 60).ToString("00");
            string currentSeconds = Mathf.Floor(currentTime % 60).ToString("00");

            string totalMinutes = Mathf.Floor(totalTime / 60).ToString("00");
            string totalSeconds = Mathf.Floor(totalTime % 60).ToString("00");

            songTimeText.text = currentMinutes + ":" + currentSeconds + " / " + totalMinutes + ":" + totalSeconds;

            string currentSongName = currentSong.name;
            // Truncate song name if it's too long
            if (currentSongName.Length > 30)
            {
                currentSongName = currentSongName.Substring(0, 30) + "...";
            }     
            songTitleText.text = currentSongName;

            string nextUpName = queue[(currentSongIndex + 1) % queue.Count].name;
            if (nextUpName.Length > 45)
            {
                nextUpName = nextUpName.Substring(0, 45) + "...";
            }
            nextUpText.text = "Next up: " + nextUpName;
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
                audioSource.clip = currentSong.clip;

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

    public void SetQueue(List<Song> newQueue)
    {
        queue = newQueue;

        // Set the default track
        queue.Insert(0, new Song("One Kiss (Calvis Harris)", clip));
        currentSong = queue[currentSongIndex];

        SetQueueText();
    }

    private void SetQueueText()
    {
        string queueString = "";
        for (int i = currentSongIndex; i < queue.Count; i++)
        {
            if (i - currentSongIndex < MAX_DISPLAYED_SONGS && i == currentSongIndex)
            {
                queueString += "<color=#fff07a>" + queue[i].name + "</color>\n";
            }
            else if (i - currentSongIndex < MAX_DISPLAYED_SONGS)
            {
                queueString += queue[i].name + "\n";
            }
            else
            {
                queueString += "...";
                break;
            }
        }

        foreach (TextMeshProUGUI queueText in queueTexts)
        {
            queueText.text = queueString;
        }
    }

    public Song GetCurrentSong()
    {
        return currentSong;
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
}
