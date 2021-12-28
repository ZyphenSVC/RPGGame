using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    
    public string name;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;
    public SpriteRenderer sr;
    public bool display;
    public bool walkAnimation;
    
    private string direction;
    private int frames;

    private void Start() {
        direction = "down";
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = frontSprite;
        frames = 15;
        display = true;
    }

    public void setChar() {
        sr.sprite = frontSprite;
    }

    public void changeDirection(string dir) {
        direction = dir;
        switch (direction) {
            case "up":
                sr.sprite = backSprite;
                break;
            case "down":
                sr.sprite = frontSprite;
                break;
            case "left":
                sr.flipX = false;
                sr.sprite = sideSprite;
                break;
            case "right":
                sr.flipX = true;
                sr.sprite = sideSprite;
                break;
        }
    }

    public string getDirection() {
        return direction;
    }

    public IEnumerator walk() {
        float wait = 0.1318f;
        walkAnimation = true;
        switch (direction) {
            case "up":
                sr.sprite = backSprite;
                yield return new WaitForSeconds(wait);
                sr.sprite = backSprite;
                yield return new WaitForSeconds(wait);
                break;
            case "down":
                sr.sprite = frontSprite;
                yield return new WaitForSeconds(wait);
                sr.sprite = frontSprite;
                yield return new WaitForSeconds(wait);
                break;
            case "left":
                sr.flipX = false;
                sr.sprite = sideSprite;
                yield return new WaitForSeconds(wait);
                sr.flipX = false;
                sr.sprite = sideSprite;
                yield return new WaitForSeconds(wait);
                break;
            case "right":
                sr.flipX = true;
                sr.sprite = sideSprite;
                yield return new WaitForSeconds(wait);
                sr.flipX = true;
                sr.sprite = sideSprite;
                yield return new WaitForSeconds(wait);
                break;
        }

        walkAnimation = false;
    }

    private void Update() {
        if (display && sr.enabled == false)
            sr.enabled = true;
        else if (!display)
            sr.enabled = false;
        frames += 1;
    }
}
