using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    private float timer = 0;
    private float timeMove = 0.5f;
    private int totMove = 0;
    float speed = 0.25f;
    private Rigidbody2D rb2;
    
    public GameObject enemyProjectile;
    public GameObject enemyProjectileClone;
    public GameObject enemy;
    
    private State state;
    
    private enum State {
        Playing,
        Dead,
        Win,
    }
    
    private static Enemy instance;

    public static Enemy GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        state = State.Playing;
    }

    private void Start() {
        Player.GetInstance().died += onDied;
        Player.GetInstance().winning += onWin;
    }

    void onDied(object sender, System.EventArgs e) {
        state = State.Dead;
    }

    void onWin(object sender, System.EventArgs e) {
        state = State.Win;
    }
    
    // Update is called once per frame
    void Update() {
        if (state == State.Playing) {
            if (GameHandler.playGame) {
                if (totMove == 14) {
                    transform.Translate(new Vector3(0, -1f, 0));
                    totMove = -1;
                    speed = -speed;
                    timer = 0;
                }

                timer += Time.deltaTime;
                if (timer > timeMove && totMove < 14) {
                    transform.Translate(new Vector3(speed, 0, 0));
                    timer = 0;
                    totMove++;
                }
                fireEnemyProjectile();

                if (GameHandler.lives < 1) {
                    state = State.Dead;
                }
            }
        }
    }

    void fireEnemyProjectile() {
        if (Random.Range(0f, 5000f) < 1) {
            enemyProjectileClone = Instantiate(enemyProjectile, new Vector3(enemy.transform.position.x, enemy.transform.position.y - 0.4f, 0), enemy.transform.rotation) as GameObject;
        }
    }
}
