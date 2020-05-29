﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTile : MonoBehaviour
{
    private List<GameObject> adjacentWalls = new List<GameObject>();
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.eventMask = 1; //  OnMouseDown only triggers for Layer 0: Default  // mainCamera.eventMask & (1 << 10)  OnMouseDown ignores Layer 10: Hospital Barriers
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        // Debug.Log(gameObject.name);
        if (GameManager.instance.isGameActive)
        {
            foreach (GameObject wall in adjacentWalls)
            {
                wall.transform.RotateAround(transform.position, Vector3.up, 90f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatableWall"))
        {
            adjacentWalls.Add(collision.gameObject);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatableWall"))
        {
            adjacentWalls.Remove(collision.gameObject);
        }
    }
}
