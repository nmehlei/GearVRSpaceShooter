using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsArrayController : MonoBehaviour {
    public GameObject shot;
    public Transform leftShotSpawnTransform;
    public Transform rightShotSpawnTransform;

    public float fireRate = 0.5f;
    private float nextFire = 0.5f;

    private bool isLeft = true;

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
