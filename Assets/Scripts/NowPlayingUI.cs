using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NowPlayingUI : MonoBehaviour
{
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI songTimeText;
    public Image playPauseImage;
    public Sprite playSprite;
    public Sprite pauseSprite;

    public MusicControls musicControls;

    // Start is called before the first frame update
    void Start()
    {
        songTitleText.text = "---";
        songTimeText.text = "-:-- / -:--";
    }

    // Update is called once per frame
    void Update()
    {
        string[] metadata = musicControls.GetCurrentSongMetadata();
        // Truncate title text if too long
        if (metadata[0].Length > 30)
        {
            songTitleText.text = metadata[0].Substring(0, 30) + "...";
        }
        else
        {
            songTitleText.text = metadata[0];
        }
        
        songTimeText.text = metadata[1];

        if (musicControls.nowPlaying)
        {
            playPauseImage.sprite = pauseSprite;
        }
        else
        {
            playPauseImage.sprite = playSprite;
        }
    }
}
