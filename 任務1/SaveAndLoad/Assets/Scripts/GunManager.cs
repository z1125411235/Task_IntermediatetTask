using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

    private AudioSource gunAudio;
    //最大和最小的x,y軸的旋轉角度
    private float maxYRotation = 120;
    private float minYRotation = 0;
    private float maxXRotation = 60;
    private float minXRotation = 0;
    //射擊的間隔時常
    private float shootTime = 1;
    private float shootTimer = 0;
    //子彈的遊戲物體 和子彈的生成位置
    public GameObject bulletGO;
    public Transform firePosition;

    private void Awake()
    {
        gunAudio = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //遊戲是非暫停狀態才可進行射擊 並且槍隨著滑鼠移動
        if (GameManager._inStance.isPaused == false)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                //點擊滑鼠左鍵 進行射擊
                if (Input.GetMouseButtonDown(0))
                {
                    //實體化子彈
                    GameObject bulletCurrent = GameObject.Instantiate(bulletGO, firePosition.position, Quaternion.identity);
                    //通過剛體組件給子彈添加一個正前方向上的力 以達到讓子彈向前運動的效果
                    bulletCurrent.GetComponent<Rigidbody>().AddForce(transform.forward * 3000);
                    //播放手槍開火的動畫
                    gameObject.GetComponent<Animation>().Play();
                    shootTimer = 0;
                    //播放手槍的音效
                    gunAudio.Play();

                    //增加射擊數
                    UIManager._instance.AddShootNum();
                }
            }

            //根據滑鼠在螢幕上的位置 去相對應的旋轉手槍
            float xPosPrecent = Input.mousePosition.x / Screen.width;
            float yPosPrecent = Input.mousePosition.y / Screen.height;

            float xAngle = -Mathf.Clamp(yPosPrecent * maxXRotation, minXRotation, maxXRotation) + 15;
            float yAngle = Mathf.Clamp(xPosPrecent * maxYRotation, minYRotation, maxYRotation) - 60;

            transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
        }

    }
}
        
