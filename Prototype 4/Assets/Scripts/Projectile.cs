using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody projectileRb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileRb.AddForce(Vector3.forward * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
