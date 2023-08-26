using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NowPlayingUI : MonoBehaviour
{
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI songTimeText;
    public TextMeshProUGUI totalTimeText;
    public GameObject hintToOpenMenuText;
    public Image playPauseImage;
    public Image progressBar;
    public Sprite playSprite;
    public Sprite pauseSprite;

    public MusicControls musicControls;
    public ChangeMenus changeMenus;

    private float progressBarWidth = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        songTitleText.text = "---";
        songTimeText.text = "-:--";
        totalTimeText.text = "-:--";
    }

    // Update is called once per frame
    void Update()
    {
        string songName = musicControls.GetSongName();

        // Truncate title text if too long
        if (songName.Length > Constants.SONG_NAME_MAX_CHARS)
        {
            songTitleText.text = songName.Substring(0, Constants.SONG_NAME_MAX_CHARS) + "...";
        }
        else
        {
            songTitleText.text = songName;
        }

        // Set time text
        float[] timeMetadata = musicControls.GetTimeMetadata();
        float currentTime = timeMetadata[0];
        float totalTime = timeMetadata[1];

        string currentMinutes = Mathf.Floor(currentTime / 60).ToString("00");
        string currentSeconds = Mathf.Floor(currentTime % 60).ToString("00");
        string totalMinutes = Mathf.Floor(totalTime / 60).ToString("00");
        string totalSeconds = Mathf.Floor(totalTime % 60).ToString("00");

        songTimeText.text = currentMinutes + ":" + currentSeconds;
        totalTimeText.text = totalMinutes + ":" + totalSeconds;

        // Set progress bar width according to current time
        float progress = currentTime / totalTime;
        progressBar.rectTransform.sizeDelta = new Vector2(progress * progressBarWidth, progressBar.rectTransform.sizeDelta.y);

        // Set play/pause icon
        if (musicControls.nowPlaying)
        {
            playPauseImage.sprite = pauseSprite;
        }
        else
        {
            playPauseImage.sprite = playSprite;
        }

        // Show hint to player how to show menus if they are hidden
        if (changeMenus.IsMenuHidden())
        {
            hintToOpenMenuText.SetActive(true);
        }
        else
        {
            hintToOpenMenuText.SetActive(false);
        }
    }
}
