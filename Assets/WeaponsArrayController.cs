using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsArrayController : MonoBehaviour {

    // Units fields

    [Header("Properties")]
    public float fireRate = 0.5f;

    [Header("Templates")]
    public GameObject shot;    

    [Header("Spawn points")]
    public Transform leftShotSpawnTransform;

    public Transform rightShotSpawnTransform;

    // Fields

    private float nextFire = 0.5f;

    private bool isLeft = true;

    // Methods

    void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (isLeft)
            {
                Instantiate(shot, leftShotSpawnTransform.position, leftShotSpawnTransform.rotation);
                isLeft = false;
            }
            else
            {
                Instantiate(shot, rightShotSpawnTransform.position, rightShotSpawnTransform.rotation);
                isLeft = true;
            }            
        }
    }
}