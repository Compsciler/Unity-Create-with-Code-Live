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
        if (transform.position.z > zTopBound || transform.position.z < zBottomBound)
        {
            Destroy(gameObject);
        }
    }
}
