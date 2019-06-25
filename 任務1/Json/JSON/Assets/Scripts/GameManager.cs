using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Text SaveText;
    public Text LoadText;

    public Character id = new Character("");
    JsonData playerJson;

    public void Save()
    {
        id = new Character(SaveText.text);
        playerJson = JsonMapper.ToJson(id);
        StreamWriter file = new StreamWriter(Path.Combine(Application.streamingAssetsPath, "ID.json"));
        file.Write(playerJson);
        file.Close();
    }

    public void Load()
    {
        //string file = Path.Combine(Path.Combine(Application.dataPath, "StreamingFile"), "ID.json");
        string file = Path.Combine(Application.streamingAssetsPath, "ID.json");
        StreamReader ld = new StreamReader(file);

        if (file != null)
        {     
        string jsonStr = ld.ReadToEnd();        
        ld.Close();

        ID id = new ID();
        id = JsonUtility.FromJson<ID>(jsonStr);
        LoadText.text = id.id;

        Debug.Log(id.id);
        }
        else
        {
            ld.Close();
            Debug.Log("The data is not found.");
        }     
    }
}

public class Character
{
    public string id;


    public Character(string id)
    {
        this.id = id;
    }
   
}

[System.Serializable]
public class ID
{
    public string id;
}


