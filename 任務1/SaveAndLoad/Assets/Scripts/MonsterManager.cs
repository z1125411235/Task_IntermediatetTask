using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    private Animation anim;

    //定義 Idle狀態和Die狀態的動畫
    public AnimationClip idleClip;
    public AnimationClip dieClip;
    //撞擊音效的撥放器
    public AudioSource kickAudio;

    public int monsterType;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.clip = idleClip;
    }

    //當檢測到碰撞時
    private void OnCollisionEnter(Collision collision)
    {
        //如果碰撞到物體的標籤為Bullet 就銷毀他
        if (collision.collider.tag == "Bullet")
        {
            Destroy(collision.collider.gameObject);

            //播放撞擊音效
            kickAudio.Play();

            //把默認的動畫變為死亡動畫 並播放
            anim.clip = dieClip;
            anim.Play();
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("Deactivate");

            UIManager._instance.AddScore();
        }
    }
    
    //當怪物被Disable的時候 將默認的動畫修改為idle動畫
    private void OnDisable()
    {
        anim.clip = idleClip;
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.8f);
        //使當前的怪物變為未激活狀態，並使整個循環重新開始
        gameObject.GetComponentInParent<TargetManager>().UpdateMonsters();
    }

}
