using System.Collections;
using System.Collections.Generic;
using System.Drawing;
// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour
{
    public Camera cam;

    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    public float minDistanceRelocation;
    public float xPosRange = 45f;
    public float zPosRange = 25f;
    public float yPos = 1.67f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= minDistanceRelocation)  // Returns 0 when reached destination or when blocked, as close as it can get
        {
            float xPos = Random.Range(-xPosRange, xPosRange);
            float zPos = Random.Range(-zPosRange, zPosRange);
            Vector3 newLocation = new Vector3(xPos, yPos, zPos);
            if (CalculateNewPath(newLocation))
            {
                agent.SetDestination(newLocation);
                // Debug.Log("Path available: " + newLocation);
            }
            else
            {
                // Debug.Log("Path NOT available");
            }

            /*
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (CalculateNewPath(hit.transform.position))
                {
                    agent.SetDestination(hit.point);
                    Debug.Log("Path available: " + hit.transform.position);
                }
                else
                {
                    Debug.Log("Path NOT available");
                }
            }
            */

            /*
            NavMeshHit hit2;
            if (NavMesh.SamplePosition(hit.transform.position, out hit2, 1f, NavMesh.AllAreas))
            {
                UnityEngine.Vector3 result = hit2.position;
                Debug.Log(result);
            }
            else
            {
                Debug.Log(false);
            }
            */
            // OPTIONS: CalculatePath every frame, every second, or every wall rotated
            // OTHER OPTIONS: Move Random Direction until hit wall and repeat
            // https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html
            // NavMeshPath path = new NavMeshPath();
            // bool hasPath = NavMesh.CalculatePath(transform.position, hit.transform.position, NavMesh.AllAreas, path);
            // How to have healthy people avoid going on top of hospital tile in Normal difficulty: https://www.youtube.com/watch?v=CHV1ymlw-P8
        }
    }
    bool CalculateNewPath(Vector3 targetPos)
    {
        agent.CalculatePath(targetPos, navMeshPath);
        // Debug.Log("New path calculated");
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        return true;
    }
}
