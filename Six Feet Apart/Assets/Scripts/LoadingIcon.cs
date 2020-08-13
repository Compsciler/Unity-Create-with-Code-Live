using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour
{
    public float rotationSpeed;
    public bool isRandomDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (isRandomDirection)
        {
            if (Random.Range(0, 2) == 1)
            {
                rotationSpeed = -rotationSpeed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
