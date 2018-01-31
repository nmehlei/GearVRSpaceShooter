using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// adds destruction behaviour to an asteroid, so it can be destroyed by weapons fire (= projectiles)
/// </summary>
public class AsteroidDestructionHandler : MonoBehaviour {

    // Constants

    public const int DefaultHealth = 5;
    
    // Unity Properties

    [Tooltip("a single projectile hit will count as 1 lost health point.")]
    public int Health = DefaultHealth;

    // Methods

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Health--;

            // Destroy the projectile since it hit the asteroid
            Destroy(other.gameObject);

            if (Health == 0)
            {
                // Destroy the asteroid aswell when its health reached zero
                Destroy(gameObject);                
            }
        }
    }
}
