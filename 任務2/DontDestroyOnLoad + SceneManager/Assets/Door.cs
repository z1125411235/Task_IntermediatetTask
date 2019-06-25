using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Door : MonoBehaviour {
    [Header("連接到某場景")]
    public string goToTheScene;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("玩家")){
            SceneManager.LoadScene(goToTheScene);
        } 
    }

}