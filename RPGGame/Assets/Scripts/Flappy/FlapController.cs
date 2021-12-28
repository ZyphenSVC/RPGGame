using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapController : MonoBehaviour {
    private Rigidbody2D rb2;
    public const float JUMP_AMNT = 75f;
    public event EventHandler died;
    public event EventHandler startPlaying;
    public event EventHandler winning;
    private State state;
    private int score;
    
    private enum State {
        Waiting,
        Playing,
        Dead,
        Win,
    }
    
    private static FlapController instance;

    public static FlapController GetInstance() {
        return instance;
    }
    
    private void Awake() {
        instance = this;
        rb2 = GetComponent<Rigidbody2D>();
        rb2.bodyType = RigidbodyType2D.Static;
        state = State.Waiting;
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            default:
            case State.Waiting:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    state = State.Playing;
                    rb2.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (startPlaying != null) startPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    Jump();
                break;
            case State.Dead:
                break;
            case State.Win:
                break;
        }

        if (rb2.transform.position.y < -60) {
            rb2.bodyType = RigidbodyType2D.Static;
            if (died != null) died(this, EventArgs.Empty);
        }
        
        score = PipeController.GetInstance().GetPassCount();
        if (score >= 20) {
            rb2.bodyType = RigidbodyType2D.Static;
            if (winning != null) winning(this, EventArgs.Empty);
        }
    }

    void Jump() {
        rb2.velocity = Vector2.up * JUMP_AMNT;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        rb2.bodyType = RigidbodyType2D.Static;
        if (died != null) died(this, EventArgs.Empty);
    }
}
