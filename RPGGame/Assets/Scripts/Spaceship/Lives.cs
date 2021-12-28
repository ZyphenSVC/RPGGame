using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {
    private Text lifeText;
    
    private void Awake() {
        lifeText = transform.Find("lives").GetComponent<Text>();
    }

    private void Update() {
        lifeText.text = "Lives: " + GameHandler.lives.ToString();
    }
}