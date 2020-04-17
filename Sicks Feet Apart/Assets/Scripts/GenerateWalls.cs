using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWalls : MonoBehaviour
{
    public float xTileRange = 40f;
    public float zTileRange = 20f;
    public float tileSize = 10f;

    [Range(0, 100)]
    public int wallTotal;  // 40 + 64 - 4 = 100 is maximum wall total
    public GameObject wallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> tilePosList = new List<Vector3>();

        for (float x = -xTileRange; x <= xTileRange; x += tileSize)
        {
            for (float z = -zTileRange; z <= zTileRange; z += tileSize)
            {
                if (!((Mathf.Abs(x) == 0 && Mathf.Abs(z) == zTileRange) || (Mathf.Abs(x) == xTileRange && Mathf.Abs(z) == 0)))
                {
                    tilePosList.Add(new Vector3(x, wallPrefab.transform.position.y, z));
                }
            }
        }

        List<GameObject> walls = new List<GameObject>();
        for (int i = 0; i < wallTotal; i++){
            Vector3 tilePos = tilePosList[Random.Range(0, tilePosList.Count)];
            GameObject wall = Instantiate(wallPrefab, tilePos, Quaternion.identity, transform);

            // Rotate wall to one of four orientations
            wall.transform.Translate(Vector3.forward * tileSize / 2);
            wall.transform.RotateAround(tilePos, Vector3.up, Random.Range(0, 4) * 90);

            foreach (GameObject wall2 in walls)
            {
                if (wall2.transform.position == wall.transform.position)  // wall2.transform.position.Equals(wall.transform.position
                {
                    goto REPEAT;  // Essentially continues outer loop if position matches
                }
            }
            wall.GetComponent<BoxCollider>().enabled = true;
            walls.Add(wall);
            continue;

            REPEAT:
            Destroy(wall);
            i--;
        }

        Debug.Log(new Vector3(0, 0, 0) == new Vector3(0, 0, 0));
        Debug.Log(new Vector3(0, 0, 0) == new Vector3(0, 1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
