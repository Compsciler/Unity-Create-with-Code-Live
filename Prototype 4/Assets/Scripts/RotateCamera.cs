using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float keyboardRotationSpeed;
    public float mouseRotationSpeed;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isKeyboardControl)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * keyboardRotationSpeed * Time.deltaTime);
        }
        else
        {
            horizontalInput = Input.GetAxis("Mouse ScrollWheel");
            transform.Rotate(Vector3.up, horizontalInput * mouseRotationSpeed * Time.deltaTime);
        }
    }
}
