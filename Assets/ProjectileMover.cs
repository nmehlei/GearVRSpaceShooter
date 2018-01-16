using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{

    public float speed;

	void Start ()
	{
	    var rigidbody = GetComponent<Rigidbody>();
	    rigidbody.velocity = transform.forward * speed;
	}
	
	void Update ()
	{
        // movement without using unity physics engine (as in rigidbody/velocity) so as to not having to include Projectile inside 
        // physics calculation
	    //var transform = GetComponent<Transform>();
        //transform.Translate(transform.forward * 3 * Time.deltaTime);
	}
}
