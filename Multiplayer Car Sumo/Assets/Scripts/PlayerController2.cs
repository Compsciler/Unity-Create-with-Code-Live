using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;
    public float maxSpeed = 50;
    public float accelSpeed;

    public float turnSpeed = 60;

    private string forwardAxisName;
    private string horizontalAxisName;

    private float forwardInput;
    private float horizontalInput;
  

    // Start is called before the first frame update
    void Start()
    {
        forwardAxisName = "Vertical" + transform.name.Replace("Vehicle ", "");
        horizontalAxisName = "Horizontal" + transform.name.Replace("Vehicle ", "");
    }

    // Update is called once per frame
    void Update()
    {
        // Axis setup
        forwardInput = Input.GetAxis(forwardAxisName);
        horizontalInput = Input.GetAxis(horizontalAxisName);

        // Acceleration
        speed += accelSpeed * forwardInput;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        } else if (speed < -maxSpeed){
            speed = -maxSpeed;
        }

        // Move vehicle
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // transform.Translate(Vector3.right * turnSpeed * horizontalInput * Time.deltaTime);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        Debug.Log("FPS: " + 1 / Time.deltaTime);
    }
}