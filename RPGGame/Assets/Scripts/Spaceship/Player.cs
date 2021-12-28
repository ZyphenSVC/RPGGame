using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject projectile;
    public GameObject projectileClone;
    public GameObject player;
    public GameObject enemy;
    public event EventHandler died;
    public event EventHandler winning;
    private State state;
    private Rigidbody2D rb2;

    private enum State {
        Playing,
        Dead,
        Win,
    }
    
    private static Player instance;

    public static Player GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            default:
            case State.Playing:
                movement();
                fireProj();
                break;
            case State.Dead:
                break;
            case State.Win:
                break;
        }

        if (GameHandler.lives < 1) {
            rb2.bodyType = RigidbodyType2D.Static;
            if (died != null) died(this, EventArgs.Empty);
        }

        if (enemy.transform.childCount == 0) {
            rb2.bodyType = RigidbodyType2D.Static;
            if (winning != null) winning(this, EventArgs.Empty);
        }
    }

    void movement() {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(new Vector3(0, -5 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(new Vector3(5 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(new Vector3(0, 5 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(new Vector3(-5 * Time.deltaTime, 0, 0));
        }
    }

    void fireProj() {
        if (Input.GetKeyDown(KeyCode.Space) && projectileClone == null) {
            projectileClone = Instantiate(projectile, new Vector3(player.transform.position.x, player.transform.position.y + 0.8f, 0), player.transform.rotation) as GameObject;
        }
    }
}
