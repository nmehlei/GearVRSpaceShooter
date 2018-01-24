using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsArrayController : MonoBehaviour {

    // Constants

    public static readonly float DefaultFireRate = 0.5f;

    // Units fields

    [Header("Properties")]
    [Tooltip("The rate in seconds after which a new projectile is fired.")]
    public float FireRate = DefaultFireRate;

    [Header("Templates")]
    [Tooltip("The template/prefab that is being used as a projectile.")]
    public GameObject Shot;    
    
    [Header("Spawn points")]
    [Tooltip("Spawn point for projectiles of the left weapon.")]
    public Transform LeftShotSpawnTransform;

    [Tooltip("Spawn point for projectiles of the right weapon.")]
    public Transform RightShotSpawnTransform;

    // Fields

    private float _nextFire;
    private bool _isLeft = true;

    // Methods

    void Start()
    {
        _nextFire = FireRate;
    }

    void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && Time.time > _nextFire)
        {
            _nextFire = Time.time + FireRate;

            FireProjectile();         
        }
    }

    /// <summary>
    /// Fire a projectile at either the left or right weapon
    /// </summary>
    private void FireProjectile()
    {
        // toggle between left and right shot spawn position
        if (_isLeft)
        {
            Instantiate(Shot, LeftShotSpawnTransform.position, LeftShotSpawnTransform.rotation);
            _isLeft = false;
        }
        else
        {
            Instantiate(Shot, RightShotSpawnTransform.position, RightShotSpawnTransform.rotation);
            _isLeft = true;
        }
    }
}