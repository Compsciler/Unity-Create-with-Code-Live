using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15;
    public float turnSpeed = 40;

    private float forwardInput;
    private float horizontalInput;
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Axis setup
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Move vehicle
        transform.Translate(Vector3.forward * speed * forwardInput * Time.deltaTime);
        // transform.Translate(Vector3.right * turnSpeed * horizontalInput * Time.deltaTime);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        Debug.Log("FPS: " + 1 / Time.deltaTime);
    }
}