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

    public bool isTranslate;

    void Start()
    {
        
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (isTranslate)
        {
            transform.Translate(Vector3.right * translateSpeed * horizontalInput * Time.deltaTime);
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

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectilePrefabs[0], transform.position, projectilePrefabs[0].transform.rotation);
        } 
        else if (Input.GetButtonDown("Fire2"))
        {
            Instantiate(projectilePrefabs[1], transform.position, projectilePrefabs[1].transform.rotation);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Instantiate(projectilePrefabs[2], transform.position, projectilePrefabs[2].transform.rotation);
        }
    }
}
