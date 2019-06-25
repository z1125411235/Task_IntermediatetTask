using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    private void Start()
    {
        StartCoroutine("DestorySelf");
    }

    IEnumerator DestorySelf()
    {
        //等待2秒消毀子彈
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
