using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour {

    public List<GameObject> Modules = new List<GameObject>();

    private List<GameObject> SpawnPoints = new List<GameObject>();

	// Use this for initialization
	void Start () {
        SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ModuleLoc"));

        foreach (var SpawnPoint in SpawnPoints)
        {
            Instantiate(Modules[Random.Range(0, Modules.Count)], SpawnPoint.transform.position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
