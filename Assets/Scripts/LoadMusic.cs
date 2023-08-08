
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string folderName = "Around Sound TESTING";
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
