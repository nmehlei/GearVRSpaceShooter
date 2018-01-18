using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCollisionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        /*Debug.LogWarning(collision. .tag);
        if (collision.gameObject.tag == "Projectile")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            //Physics.IgnoreCollision(collision.gameObject.col, collision.collider);
            //Physics.IgnoreCollision(theobjectToIgnore.collider, collider);
        }
        else if (collision.gameObject.tag == "Asteroid")
        {
            var levelManager = LevelManager.GetInstance();
            levelManager.ResetLevel();
        }*/
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SubGoalManager.GetInstance().Reset();
        }
    }*/
    }
