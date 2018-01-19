using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    // Unity properties

    [Tooltip("The speed with which the projectile will travel.")]
    public float speed;

    // Methods

	void Start ()
	{
	    var rigidbody = GetComponent<Rigidbody>();
	    rigidbody.velocity = transform.forward * speed;
	}
	
	//void Update ()
	//{
        // movement without using unity physics engine (as in rigidbody/velocity) so as to not having to include Projectile inside 
        // physics calculation
	    //var transform = GetComponent<Transform>();
        //transform.Translate(transform.forward * 3 * Time.deltaTime);
	//}
}