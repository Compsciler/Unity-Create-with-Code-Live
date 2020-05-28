using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HospitalBarrier : MonoBehaviour
{
    private int infectedAgentID = -334000983;
    private int recentlyHealedAgentID = 1479372276;
    private int priorityInfectedAgentID = -1923039037;

    private Vector3 hospitalTilePos = new Vector3(0, 1.67f, 0);

    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Agent has isTrigger enabled
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
    
    void OnTriggerStay(Collider other)  // Changed from OnTriggerEnter  // Will this trigger once for multiple GameObjects?
    {
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log(other.gameObject);
        if (agent == null)
        {
            return;
        }
        Debug.Log(agent.agentTypeID);
        Debug.Log(HospitalTile.isOccupied);
        Debug.Log("COLLISION");
        if (agent.agentTypeID == infectedAgentID && !HospitalTile.isOccupied)  // agent.agentTypeID == recentlyHealedAgentID && !HospitalTile.isOccupied  // Change for different difficulty settings
        {
            agent.agentTypeID = priorityInfectedAgentID;
            agent.SetDestination(hospitalTilePos);
            // Physics.IgnoreCollision(GetComponent<BoxCollider>(), other, true);  // Need a NavMeshModifier Ignore
            Debug.Log("IGNORED");
            HospitalTile.isOccupied = true;
            // agent.SetDestination(new Vector3(0, 1.67f, 0));
        }
    }
}
