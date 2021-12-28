using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WarpTiles : MonoBehaviour
{
    public string warp_to;
    
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Player")
            SceneManager.LoadScene(warp_to);
    }
}
