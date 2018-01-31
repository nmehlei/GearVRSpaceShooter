using System;
using UnityEngine;

/// <summary>
/// adds self-destruction behaviour to a projectile after a specified timeout (for obvious performance reasons)
/// </summary>
public class ProjectileDestroyer : MonoBehaviour {

    // Constants

    public const int DefaultMaxAgeInSeconds = 45;

    // Unity fields

    [Tooltip("Seconds after which the projectile vanishes.")]
    public int MaxAgeInSeconds = DefaultMaxAgeInSeconds;

    // Fields

    private DateTime _startDate;

    // Methods
    
	void Start ()
	{
	    _startDate = DateTime.UtcNow;
	}
	
	void Update ()
	{
	    var age = DateTime.UtcNow - _startDate;
	    if (age.TotalSeconds > MaxAgeInSeconds)
	    {
	        Destroy(this);
	    }
	}
}