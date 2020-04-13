using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float horizontalInput;
    public float translateSpeed;
    public float rotateSpeed;

    public float xRange;

    public GameObject[] projectilePrefabs;  // Size 3

    public bool isAlternatingInput;
    public bool isTranslate;

    void Start()
    {
        
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (isAlternatingInput)
        {
            isTranslate = (DetectCollisions.isTranslateSwitchOn) ? true : false;
        }
        if (isTranslate)  // Alternate between turning and sliding player after hit if isAlternatingInput is true
        {
            transform.Translate(Vector3.right * translateSpeed * horizontalInput * Time.deltaTime, Space.World);
            if (transform.position.x < -xRange)
            {
                transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > xRange)
            {
                transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
            }
        }
        else
        {
            transform.Rotate(0, rotateSpeed * horizontalInput * Time.deltaTime, 0);
        }

        Quaternion projectileDirection;
        if (isTranslate)
        {
            projectileDirection = Quaternion.identity;
        } else
        {
            projectileDirection = transform.rotation;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectilePrefabs[0], transform.position, projectileDirection);
        } 
        else if (Input.GetButtonDown("Fire2"))
        {
            Instantiate(projectilePrefabs[1], transform.position, projectileDirection);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Instantiate(projectilePrefabs[2], transform.position, projectileDirection);
        }
    }
}
