using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour
{
    public Camera cam;

    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
            Debug.Log("Has Path: " + agent.hasPath);

            if (CalculateNewPath(hit.transform.position))
            {
                Debug.Log("Path available");
            }
            else
            {
                Debug.Log("Path NOT available");
            }
            if (agent.remainingDistance < 0.1)  // Returns 0 when reached destination or when blocked, as close as it can get
            {
                
            }
            
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
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, hit.transform.position, NavMesh.AllAreas, path);
        }
    }
    bool CalculateNewPath(UnityEngine.Vector3 targetPos)
    {
        agent.CalculatePath(targetPos, navMeshPath);
        Debug.Log("New path calculated");
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        return true;
    }
}
