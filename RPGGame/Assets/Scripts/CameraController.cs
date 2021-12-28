using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Transform player;
    int last_loaded;

    // Start is called before the first frame update
    void Awake()
    {
        transform.position = new Vector3(0, 0, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        int countLoaded = SceneManager.sceneCount;
        if(last_loaded != countLoaded){
            if(countLoaded == 1){
                transform.parent = player;
                transform.localPosition = new Vector3(0, 0, -1f);
                GetComponent<Camera>().orthographicSize = 1f;
            }
            else{
                transform.localPosition = new Vector3(-1f, 0, -1f);
                GetComponent<Camera>().orthographicSize = 4f;
            }
        }
        last_loaded = countLoaded;
    }
}
