using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    //保存所有該目標下的怪物
    public GameObject[] monsters;
    //保存目前處於激活狀態下的怪物
    public GameObject activeMonster = null;
    //表示目標所在的位置(0-8)
    public int targetPosition;

    private void Start()
    {
        foreach(GameObject monster in monsters)
        {            
            monster.GetComponent<BoxCollider>().enabled = false;
            monster.SetActive(false);
        }
        //調用協成
        StartCoroutine("AliveTimer");
    }

    //隨機產生怪物
    private void ActivateMonster()
    {
        int index = Random.Range(0, monsters.Length);
        activeMonster = monsters[index];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        //調用死亡時間的協成
        StartCoroutine("DeathTimer");
    }
    //迭代器方法 ，設置生成的等待時間
    IEnumerator AliveTimer()
    {
        //等待1~4秒後執行ActivateMonster方法
        yield return new WaitForSeconds(Random.Range(1, 5));
        ActivateMonster();
    }

    //使激活狀態的怪物變為未激活狀態
    private void DeActivateMonster()
    {
        if(activeMonster != null)
        {        
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        //調用激活時間的協成,達到反覆激活和死亡的循環
        StartCoroutine("AliveTimer");
    }

    //迭代器 設置死亡時間
    IEnumerator DeathTimer()
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            DeActivateMonster();
        }

    //更新生命週期 當子彈擊中怪物時 或著當重新開始遊戲時
    //停止所有協成
    //將當前處於激活狀態的怪物變為未激活狀態 清空activeMonster
    //重新開始AliveTimer的協成 (隨機激活怪物)
    public void UpdateMonsters()
    {
        StopAllCoroutines();
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        StartCoroutine("AliveTimer");
    }

    //按照給定的怪物類型激活怪物
    //停止所有協成
    //將當前激活狀態的怪物 (如果有的話) 轉變為未激活狀態
    //激活給定的類型的怪物
    //調用死亡時間的協成
    public void ActivateMonsterByType(int type)
    {
        StopAllCoroutines();
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        activeMonster = monsters[type];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("DeathTimer");
    }
}
