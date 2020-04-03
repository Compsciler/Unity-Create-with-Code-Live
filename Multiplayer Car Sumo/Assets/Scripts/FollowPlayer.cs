using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;  // Make private?
    public Vector3 finalRotation;  // Make private?

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerController.isWon)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            transform.position = player.transform.position + offset;
            transform.rotation = Quaternion.Euler(finalRotation);
        }
    }
}
