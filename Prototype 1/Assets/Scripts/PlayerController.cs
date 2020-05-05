using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Vehicle Stats")]
    // [SerializeField] float speed = 15;
    [SerializeField] float power = 10000;
    [SerializeField] float turnSpeed = 40;

    private float forwardInput;
    private float horizontalInput;

    Rigidbody playerRb;

    [SerializeField] GameObject centerOfMass;

    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI gearText;
    int gear;
    int gearInterval = 8;

    [SerializeField] List<WheelCollider> allWheels;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOnGround())
        {
            // Axis setup
            forwardInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            // Move vehicle
            playerRb.AddRelativeForce(Vector3.forward * power * forwardInput);
            // transform.Translate(Vector3.forward * speed * forwardInput * Time.deltaTime);
            // transform.Translate(Vector3.right * turnSpeed * horizontalInput * Time.deltaTime);
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

            speed = playerRb.velocity.magnitude;
            int mph = (int)(speed * 2.237f);  // 3.6f for kph
            speedometerText.text = "Speed: " + mph + " mph";

            if (mph % gearInterval == 0)
            {
                gear = mph / gearInterval + 1;
                gearText.SetText("Gear: " + gear);
            }
        }
        Debug.Log("FPS: " + 1 / Time.deltaTime);
    }
    bool isOnGround()
    {
        foreach (WheelCollider wheel in allWheels)
        {
            if (!wheel.isGrounded)
            {
                return false;
            }
        }
        return true;
    }
}