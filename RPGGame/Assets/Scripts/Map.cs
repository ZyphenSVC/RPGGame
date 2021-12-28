using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform entry_position;

    public string travel_mode;

    public bool use_stairs;

    public Transform stair_entry_position;
    
    private PlayerController p;
    
    public bool suppress_map_just_changed;
    
    public bool encounters;
    
    public bool play_music;
    
    void Awake(){
        p = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
    }
}
