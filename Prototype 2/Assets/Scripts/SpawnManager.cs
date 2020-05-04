using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject[] animalPrefabs;
	private float spawnRangeX = 20;
	private float spawnPosZ = 20;

	private float startDelay = 2f;
	private float spawnInterval = 2.2f;


	private void Start()
	{
		InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
	}

	void SpawnRandomAnimal()
	{
		int animalIndex = Random.Range(0, animalPrefabs.Length);

		Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);

		Instantiate(animalPrefabs[animalIndex],
			spawnPos,
			animalPrefabs[animalIndex].transform.rotation);
	}
}
