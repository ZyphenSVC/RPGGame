using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed;
    public Transform point;
    public bool canMove;
    public LayerMask collision;
    public LayerMask NPC;
    public GameObject pauseMenu;
    public SpriteController sc;
    public MapHandler map_handler;
    public bool map_just_changed;
    public int frames;
    public float multiplier = 2f;
    public OverworldGrid og;
    private float timer = -1;
    private List<float> times;
    
    private void OnEnable() {
        Cursor.visible = false;
        if (canMove)
            pauseMenu.SetActive(true);
    }

    private void Start() {
        point.parent = transform.parent;
        point.transform.position = transform.position;
        canMove = true;
        frames = 0;
        
    }

    void startTimer() {
        if (times == null)
            times = new List<float>();
        timer = 0;
    }

    void stopTimer() {
        times.Add(timer);
        float total = 0f;
        foreach (var f in times)
            total += f;
        timer = -1f;
    }

    private void Update() {
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.MoveTowards(transform.position, point.position, moveSpeed * Time.deltaTime);
        if (canMove) {
            if (Vector3.Distance(transform.position, point.position) <= .025f) {
                bool up = Input.GetKey(CustomInputManager.cim.up);
                bool down = Input.GetKey(CustomInputManager.cim.down);
                bool left = Input.GetKey(CustomInputManager.cim.left);
                bool right = Input.GetKey(CustomInputManager.cim.right);
                float vert = 0f;
                float hori = 0f;

                if (up)
                    vert = 1f;
                else if (down)
                    vert = -1f;
                else if (left)
                    hori = -1f;
                else if (right)
                    hori = 1f;

                vert *= multiplier;
                hori *= multiplier;

                if (!up && !down && !left && !right && transform.position == point.position)
                    switch (sc.getDirection()) {
                        case "up":
                            sc.changeDirection("up");
                            break;
                        case "down":
                            sc.changeDirection("down");
                            break;
                        case "left":
                            sc.changeDirection("left");
                            break;
                        case "right":
                            sc.changeDirection("right");
                            break;
                    }
                else if (Mathf.Abs(hori) == 1f * multiplier) {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(hori, 0f, 0f), 1f * multiplier,
                        collision);
                    if (hori == 1f * multiplier)
                        sc.changeDirection("right");
                    if (hori == -1f * multiplier)
                        sc.changeDirection("left");
                    if (hit.collider == null || hit.collider.gameObject.layer == 0 || hit.collider.isTrigger) {
                        point.position += new Vector3(hori * Time.deltaTime, 0f, 0f);
                        StartCoroutine(sc.walk());
                    }
                } else if (Mathf.Abs(vert) == 1f * multiplier) {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0f, vert, 0f), 1f * multiplier,
                        collision);
                    if (vert == 1f * multiplier)
                        sc.changeDirection("up");
                    if (vert == -1f * multiplier)
                        sc.changeDirection("down");
                    if (hit.collider == null || hit.collider.gameObject.layer == 0) {
                        point.position += new Vector3(0f, vert * Time.deltaTime, 0f);
                        StartCoroutine(sc.walk());
                    }
                }
            }
        }

        frames += 1;
        
        if (SceneManager.GetActiveScene().name == "home") {
            if (transform.position.x > 2.7)
                transform.position = new Vector3((float) -2.7, transform.position.y, 0);
            else if (transform.position.x < -2.7)
                transform.position = new Vector3((float) 2.7, transform.position.y, 0);
        
            if (transform.position.y > 2.7)
                transform.position = new Vector3(transform.position.x, (float) -2.7, 0);
            else if (transform.position.y < -2.7)
                transform.position = new Vector3(transform.position.x, (float) 2.7, 0);
        }

        if (SceneManager.GetActiveScene().name == "outside") {
            if (transform.position.y > 2.3 && transform.position.x > -1.6 && transform.position.x < 0.75) 
                transform.position = new Vector3(transform.position.x, (float) 2.3, 0);
        }
        // -2, 2
    }
    

    public Vector3 getFacing() {
        Vector3 dir = Vector3.zero;
        if(sc.getDirection() == "up"){
            dir = new Vector3(0f, 1f, 0f);
        }
        else if(sc.getDirection() == "down"){
            dir = new Vector3(0f, -1f, 0f);
        }
        else if(sc.getDirection() == "left"){
            dir = new Vector3(-1f, 0f, 0f);
        }
        else if(sc.getDirection() == "right"){
            dir = new Vector3(1f, 0f, 0f);
        }
        return dir;
    }
    
    void OnTriggerEnter2D(Collider2D c){

        if(transform.position == point.position)
        {
            return;
        }
        if(c.gameObject.GetComponent<RoomHandler>()){
            c.gameObject.GetComponent<RoomHandler>().change();
        }
        else if(c.gameObject.GetComponent<OverworldGrid>()){
            og = c.gameObject.GetComponent<OverworldGrid>();
        }
        else if(map_handler.done_changing)
        {
            canMove = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D c){
        map_just_changed = false;
        if (map_handler.done_changing)
        {
            canMove = true;
        }
    }

    bool isWalkable(Vector3 tar) {
        if (Physics2D.OverlapCircle(tar, 0.3f, collision | NPC) != null)
            return false;
        return true;
    }
}
