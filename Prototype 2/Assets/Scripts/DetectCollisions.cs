using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    internal static bool isTranslateSwitchOn = true;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
        isTranslateSwitchOn = !isTranslateSwitchOn;
    }
}
