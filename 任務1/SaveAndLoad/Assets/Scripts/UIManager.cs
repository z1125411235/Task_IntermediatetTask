using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {
    public static UIManager _instance;
    //獲得2個Text組件
    public Text shootNumText;
    public Text scoreText;

    public int shootNum = 0;
    public int score = 0;
    //音樂開關的單選框播放框背景音樂的AudioSource
    public Toggle musicToggle;
    public AudioSource musicAudio;

    public Text messageText;

    private void Awake()
    {
        _instance = this;
        if (PlayerPrefs.HasKey("MusicOn"))
        {
            if (PlayerPrefs.GetInt("MusicOn") == 1)
            {
                musicToggle.isOn = true;
                musicAudio.enabled = true;
            }
            else
            {
                musicToggle.isOn = false;
                musicAudio.enabled = false;
            }
        }
        else
        {
            musicToggle.isOn = true;
            musicAudio.enabled = false;
        }
    }
    private void Update()
    {
        //更新Text組件的顯示內容
        shootNumText.text = shootNum.ToString();
        scoreText.text = score.ToString();

    }

    public void MusicSwich()
    {
        //通過判斷單選框是否被勾選上 從而來決定是否撥放背景音樂
        if(musicToggle.isOn == false)
        {
            musicAudio.enabled = false;
            //保存音樂開關的狀態 0代表關閉狀態
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            musicAudio.enabled = true;
            //保存音樂開關的狀態 1代表開啟狀態
            PlayerPrefs.SetInt("MusicOn", 1);
        }
        PlayerPrefs.Save();
    }


    //增加射擊數(當開槍時)
    public void AddShootNum()
    {
        shootNum += 1;
    }
    //增加得分(當射中怪物時)
    public void AddScore()
    {
        score += 1;
    }

    public void ShowMessage(string str)
    {
        messageText.text = str;
    }

}
