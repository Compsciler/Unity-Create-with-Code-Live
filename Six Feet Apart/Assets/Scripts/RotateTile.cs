using GreatArcStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTile : MonoBehaviour
{
    private List<GameObject> adjacentWalls = new List<GameObject>();
    Camera mainCamera;
    public AudioClip rotateSound;
    public float rotateSoundVolume;

    internal static int tileClicksRemainingToTriggerDialogue;

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
        if (GameManager.instance.isGameActive && !PauseManager.isPaused && (!GameManager.instance.isTutorial || gameObject == DialogueManager.instance.currentTileToClick))
        {
            int numWallsRotated = 0;
            foreach (GameObject wall in adjacentWalls)
            {
                wall.transform.RotateAround(transform.position, Vector3.up, 90f);
                numWallsRotated++;
            }
            if (numWallsRotated >= 1 && numWallsRotated <= 3)
            {
                AudioManager.instance.SFX_Source.PlayOneShot(rotateSound, rotateSoundVolume);
            }
            if (GameManager.instance.isTutorial)
            {
                tileClicksRemainingToTriggerDialogue--;
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
