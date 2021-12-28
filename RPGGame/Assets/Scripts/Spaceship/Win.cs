using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour {
    private void Awake() {
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GameHandler.lives = 3;
            Loader.Load(Loader.Scene.Spaceship);
        };
        transform.Find("homeBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GameHandler.lives = 3;
            Loader.Load(Loader.Scene.home);
        };
    }

    private void Start() {
        Player.GetInstance().winning += onWin;
        Hide();
    }
    
    void onWin(object sender, System.EventArgs e) {
        Cursor.visible = true;
        Show();
    }

    void Hide() {
        gameObject.SetActive(false);
    }
    
    void Show() {
        gameObject.SetActive(true);
    }
}