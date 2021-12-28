using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    public GameObject enemyProjectile;
    Vector3 respawn = new Vector3(6, 4, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -5 * Time.deltaTime, 0 ));
        if (transform.position.y >= 5 || transform.position.y <= -5)
            Destroy(enemyProjectile);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.position = respawn;
            Destroy(enemyProjectile);
            GameHandler.playGame = false;
            GameHandler.lives--;
        } 
    }
}
