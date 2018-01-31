using UnityEngine;

/// <summary>
/// adds movement behaviour to projectiles. Has dedicated mono behaviour to easily upgrade it for more advanced movement handling,
/// like add the shooting ships' speed to the projectiles' speed at time of creation.
/// </summary>
public class ProjectileMover : MonoBehaviour
{
    // Unity properties

    [Tooltip("The speed with which the projectile will travel.")]
    public float Speed;

    // Methods

	void Start ()
	{
	    var rigidBody = GetComponent<Rigidbody>();
	    rigidBody.velocity = transform.forward * Speed;
	}
}