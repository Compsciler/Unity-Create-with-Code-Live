using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15;
    public float turnSpeed = 60;

    private string forwardAxisName;
    private string horizontalAxisName;

    private float forwardInput;
    private float horizontalInput;

    public float yLosePos;

    internal static bool isWon = false;

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

        // Move vehicle
        transform.Translate(Vector3.forward * speed * forwardInput * Time.deltaTime);
        // transform.Translate(Vector3.right * turnSpeed * horizontalInput * Time.deltaTime);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        if (transform.position.y < yLosePos && !isWon)
        {
            Destroy(gameObject);
            isWon = true;
        }

        Debug.Log("FPS: " + 1 / Time.deltaTime);
    }
}