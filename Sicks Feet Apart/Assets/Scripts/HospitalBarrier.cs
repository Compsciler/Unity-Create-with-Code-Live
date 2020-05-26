using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HospitalBarrier : MonoBehaviour
{
    private int recentlyHealedAgentID = 1479372276;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log(collision.gameObject);
        if (agent == null)
        {
            return;
        }
        Debug.Log(agent.agentTypeID);
        Debug.Log(HospitalTile.isOccupied);
        if (agent.agentTypeID == recentlyHealedAgentID && !HospitalTile.isOccupied)  // Change for different difficulty settings
        {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), collision.collider, true);
            Debug.Log("IGNORED");
            HospitalTile.isOccupied = true;
            // agent.SetDestination(new Vector3(0, 1.67f, 0));
        }
    }
    */

    void OnTriggerEnter(Collider other)
    {
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log(other.gameObject);
        if (agent == null)
        {
            return;
        }
        Debug.Log(agent.agentTypeID);
        Debug.Log(HospitalTile.isOccupied);
        if (agent.agentTypeID == recentlyHealedAgentID && !HospitalTile.isOccupied)  // Change for different difficulty settings
        {
            // Physics.IgnoreCollision(GetComponent<BoxCollider>(), other, true);
            Debug.Log("IGNORED");
            HospitalTile.isOccupied = true;
            // agent.SetDestination(new Vector3(0, 1.67f, 0));
        }
    }
}
