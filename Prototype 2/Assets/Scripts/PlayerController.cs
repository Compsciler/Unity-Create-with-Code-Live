using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float horizontalInput;
    public float speed;

    public float xRange;

    public GameObject projectilePrefab;

    void Start()
    {
        
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        } 
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        if (Input.GetButtonDown("Fire"))
        {
            // Instantiate different food types to meet animals' needs

            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }
}
