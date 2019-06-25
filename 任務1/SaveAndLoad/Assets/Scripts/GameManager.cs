using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;

public class GameManager : MonoBehaviour {
    public static GameManager _inStance;

    //是否是暫停狀態
    public bool isPaused = true;
    public GameObject menuGO;

    public GameObject[] targetGOs;

    private void Awake()
    {
        _inStance = this;
        //遊戲開始時是暫停的狀態
        Pause();
    }

    private void Update()
    {
        //判斷是否按下ESC建 按下的話 調出Menu菜單 並將遊戲狀態更改為暫停狀態
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    //暫停狀態
    private void Pause()
    {
        isPaused = true;
        menuGO.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    //不暫停狀態
    private void UnPause()
    {
        isPaused = false;
        menuGO.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    //創建Save對象並存儲當前遊戲狀態訊息
    private Save CreateSaveGO()
    {
        //新建Save對象
        Save save = new Save();
        //遍歷所有的target
        //如果其中有處於激活狀態的怪物 就把該target的位置訊息和激活狀態的怪物的類型添加到List中
        foreach(GameObject targetGO in targetGOs)
        {
            TargetManager targetManager = targetGO.GetComponent<TargetManager>();
            if(targetManager.activeMonster != null)
            {
                save.livingTargetPositions.Add(targetManager.targetPosition);
                int type = targetManager.activeMonster.GetComponent<MonsterManager>().monsterType;
                save.livingMonsterTypes.Add(type);
            }
        }
        //把shootNum和score保存在Save對象中
        save.shootNum = UIManager._instance.shootNum;
        save.score = UIManager._instance.score;
        //返回該Save對象
        return save;
    }

    private void SetGame(Save save)
    {
        //先將所有的target裡面的怪物清空 並重置所有的計時
        foreach(GameObject targetGO in targetGOs)
        {
            targetGO.GetComponent<TargetManager>().UpdateMonsters();
        }
        //通過反序列化得到的Save對象中存儲的訊息 激活指定的怪物
        for(int i = 0; i < save.livingTargetPositions.Count; i++)
        {
            int position = save.livingTargetPositions[i];
            int type = save.livingMonsterTypes[i];

            targetGOs[position].GetComponent<TargetManager>().ActivateMonsterByType(type);
        }
        //更新UI顯示
        UIManager._instance.shootNum = save.shootNum;
        UIManager._instance.score = save.score;
        //調整為為暫停狀態
        UnPause();
    }

    //二進制方法 : 存檔與讀檔
    private void SaveByBin()
    {
        //序列化過程 (將Save對象轉化為字節流)
        //創建Save對象並保存當前遊戲狀態
        Save save = CreateSaveGO();
        //創建一個二進制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //創建一個文件流
        FileStream fileStream = File.Create(Application.dataPath + "/StreamingFile" + "/byBin.txt");
        //用二進制格式化程序的序列化方法來序列化Save對象 參數 : 創建的文件流和需要序列化對象
        bf.Serialize(fileStream, save);
        fileStream.Close();

        //如果文件存在 則顯示保存成功
        if (File.Exists(Application.dataPath + "/StreamingFile" + "/byBin.txt"))
        {
            UIManager._instance.ShowMessage("保存成功");
        }
    }

    private void LoadByBin()
    {
        if(File.Exists(Application.dataPath + "/StreamingFile" + "/byBin.txt"))
        {
            //反序列化過程 
            //創建二進制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打開一個文件夾
            FileStream fileStream = File.Open(Application.dataPath + "/StreamingFile" + "/byBin.txt", FileMode.Open);
            //調用格式化程序的反序列化方法 將文件轉化為一個Save對象
            Save save = (Save)bf.Deserialize(fileStream);
            //關閉文件流
            fileStream.Close();

            SetGame(save);

        }
        else
        {
            UIManager._instance.ShowMessage("存檔文件不存在");
        }
    }

    //XML : 存檔與讀檔
    private void SaveByXml()
    {

    }

    private void LoadByXml()
    {

    }

    //JSON : 存檔與讀檔
    private void SaveByJson()
    {
        Save save = CreateSaveGO();
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        //利用JsonMapper將save對象轉化為Json格式的字符串
        string saveJsonStr = JsonMapper.ToJson(save);
        //將這個字符串寫入到文件中
        //創建一個StreamWriter 並將字符串寫入文件中
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        //關閉StreamWriter
        sw.Close();

        UIManager._instance.ShowMessage("保存成功");
    }

    private void LoadByJson()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        if (File.Exists(filePath))
        {
            //創建一个StreamReader，用來讀取流
            StreamReader sr = new StreamReader(filePath);
            //將讀取到的流赋值给jsonStr
            string jsonStr = sr.ReadToEnd();
            //關閉
            sr.Close();

            //將字符串jsonStr轉化為Save對象
            Save save = JsonMapper.ToObject<Save>(jsonStr);
            SetGame(save);
            UIManager._instance.ShowMessage("");
        }
        else
        {
            UIManager._instance.ShowMessage("存檔文件不存在");
        }
    }



    //從暫停狀態恢復到不暫停狀態
    public void ContinueGame()
    {
        UnPause();
        UIManager._instance.ShowMessage("");
    }

    //重新開始遊戲
    public void NewGame()
    {
        foreach(GameObject targetGO in targetGOs)
        {
            targetGO.GetComponent<TargetManager>().UpdateMonsters();
        }
        UIManager._instance.shootNum = 0;
        UIManager._instance.score = 0;
        UIManager._instance.ShowMessage("");
        UnPause();
    }

    //退出遊戲
    public void QuitGame()
    {
        Application.Quit();
    }

    //保存遊戲
    public void SaveGame()
    {
        //SaveByBin();
        SaveByJson();
    }

    //加載遊戲
    public void LoadGame()
    {
        //LoadByBin();
        LoadByJson();
        UIManager._instance.ShowMessage("");
    }
}
