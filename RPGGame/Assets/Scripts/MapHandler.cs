using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public PlayerController player;
    public GameObject grids;
    public GameObject active_map;
    
    public float overworldX;
    public float overworldY;
    
    float submapX;
    float submapY;
    
    public bool done_changing;
    
    // Start is called before the first frame update
    void Start()
    {
        deactivate_maps_except(active_map);
        done_changing = true;
        active_map.SetActive(true);
        active_map.GetComponent<Map>().play_music = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active_map.name == "Overworld"){
            overworldX = player.transform.position.x;
            overworldY = player.transform.position.y;
            if(!active_map.GetComponent<Map>().play_music && done_changing){
                active_map.GetComponent<Map>().play_music = true;
            }
        }
        else{
            submapX = player.transform.position.x;
            submapY = player.transform.position.y;
        }
    }
    
    void deactivate_maps_except(GameObject map){
        Map[] maps = grids.GetComponentsInChildren<Map>();
        foreach(Map m in maps){
            if(m.enabled == false){
                m.play_music = false;
                continue;
            }
            if(m.gameObject.GetInstanceID() != map.GetInstanceID()){
                m.play_music = false;
                m.gameObject.SetActive(false);
            }
        }
        
        active_map = map;
    }
    
    public void change_maps(GameObject map){
        if(done_changing)
            change(map);
    }
    
    void change(GameObject map){
        done_changing = false;

        player.canMove = false;
        
        Map active = active_map.GetComponent<Map>();
        active.play_music = false;

        bool use_stairs = active.use_stairs;

        deactivate_maps_except(map);
        active = active_map.GetComponent<Map>();
        
        if(active.name != "Overworld"){
            if (use_stairs)
            {
                player.transform.position = active.stair_entry_position.position;
                player.sc.changeDirection("down");
            }
            else
            {
                player.transform.position = active.entry_position.position;
                player.sc.changeDirection("up");
            }
        }
        else{
            player.transform.position = new Vector3(overworldX, overworldY, 0f);
            player.sc.changeDirection("down");
        }
        
        player.point.position = player.transform.position;
        player.map_just_changed = true;
        
        active_map.SetActive(true);
        
        player.canMove = true;
        
        done_changing = true;
    }
    
    GameObject get_map(string map_name){
        Transform[] children = grids.transform.GetComponentsInChildren<Transform>();
        foreach(var child in children){
            if(child.GetComponent<Map>() && child.name == map_name){
                return child.gameObject;
            }
        }
        return null;
    }
    
}
