using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWin : MonoBehaviour {
    private Text scoreText;
    
    private void Awake() {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.FlappyTweet);
        transform.Find("homeBtn").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.home);
    }

    private void Start() {
        FlapController.GetInstance().winning += onWin;
        Hide();
    }
    
    void onWin(object sender, System.EventArgs e) {
        Cursor.visible = true;
        scoreText.text = PipeController.GetInstance().GetPassCount().ToString();
        Show();
    }

    void Hide() {
        gameObject.SetActive(false);
    }
    
    void Show() {
        gameObject.SetActive(true);
    }
}