using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    // Unity properties

    [Tooltip("The speed with which the projectile will travel.")]
    public float speed;

    // Methods

	void Start ()
	{
	    var rigidBody = GetComponent<Rigidbody>();
	    rigidBody.velocity = transform.forward * speed;
	}
}