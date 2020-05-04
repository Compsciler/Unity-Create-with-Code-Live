using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Instead of destroying the projectile when it collides with an animal
        //Destroy(gameObject); 

        // Just deactivate the food and destroy the animal
        gameObject.SetActive(false);
        Destroy(other.gameObject);

    }

}
