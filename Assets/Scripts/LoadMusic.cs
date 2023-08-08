
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Song
{
    public string name;
    public AudioClip clip;

    public Song(string name, AudioClip clip)
    {
        this.name = name;
        this.clip = clip;
    }
}

public class LoadMusic : MonoBehaviour
{
    private string folderName;
    private string folderPath;
    public TextMeshProUGUI queueText;

    public List<Song> queue = new List<Song>();

    // Start is called before the first frame update
    void Start()
    {
        folderName = "Around Sound";
        folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        StartCoroutine(InitializeAudioFiles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator InitializeAudioFiles()
    {
        yield return LoadAllAudioFiles();

        // Make queueText display name of all songs in audioClips (temporary)
        string queueString = "";
        foreach (Song clip in queue)
        {
            queueString += clip.name + "\n";
        }
        queueText.text = queueString;
    }

    private IEnumerator LoadAllAudioFiles()
    {
        Debug.Log("LOADING AUDIO FILES FROM: " + folderPath);
        string[] fileExtensions = { "*.wav", "*.ogg" };

        foreach (string extension in fileExtensions)
        {
            string[] files = Directory.GetFiles(folderPath, extension);
            foreach (string file in files)
            {
                yield return StartCoroutine(LoadAudioClip(file));
            }
        }

        // Log names of all audio clips
        foreach (Song clip in queue)
        {
            Debug.Log("Loaded song name: " + clip.name);
        }
    }

    IEnumerator LoadAudioClip(string filePath)
    {
        string url = "file://" + filePath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, GetAudioType(filePath)))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                string fileName = Path.GetFileNameWithoutExtension(filePath); // Get the file name without extension
                queue.Add(new Song(fileName, clip));
                Debug.Log("SUCCESSFULLY LOADED: " + filePath);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }

    private AudioType GetAudioType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".wav":
                return AudioType.WAV;
            case ".ogg":
                return AudioType.OGGVORBIS;
            default:
                Debug.LogError("Unsupported audio type: " + extension);
                return AudioType.UNKNOWN;
        }
    }
}
