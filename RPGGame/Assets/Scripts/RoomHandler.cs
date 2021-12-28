using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{
    public GameObject rooms;
    public GameObject outside_collision;
    public GameObject outside_NPCs;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }
    
    public void change(){
        rooms.SetActive(!rooms.active);
        outside_collision.SetActive(!outside_collision.active);
        outside_NPCs.SetActive(!outside_NPCs.active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
