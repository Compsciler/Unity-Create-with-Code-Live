using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float bottomSpeed = 40;
    public float topSpeed = 40;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(bottomSpeed, topSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
