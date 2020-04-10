using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float zTopBound = 30;
    public float zBottomBound = -10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > zTopBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < zBottomBound)
        {
            // Only animals will get to this block
            Destroy(gameObject);
            Debug.Log("Game Over");
        }
    }
}
