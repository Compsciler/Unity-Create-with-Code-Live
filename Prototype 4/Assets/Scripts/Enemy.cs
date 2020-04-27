using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;

    public float speed = 3f;
    private Vector3 platformCenter = new Vector3(0, 0.1f, 0);
    public float stoppingDistance = 11f;
    public float speed2 = 4f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
        if (Vector3.Distance(transform.position, platformCenter) > stoppingDistance)
        {
            Vector3 toCenterDirection = (platformCenter - transform.position).normalized;
            enemyRb.AddForce(toCenterDirection * speed2);
        }
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
