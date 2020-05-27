// using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using MEC;
using System.Threading;
// using NUnit.Framework.Internal;

public class PersonController : MonoBehaviour
{
    public Camera cam;

    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    public float minDistanceRelocation;
    public float xPosRange = 45f;
    public float zPosRange = 25f;
    public float yPos = 1.67f;

    private float tileSize = 10f;
    private Vector3 hospitalTilePos = new Vector3(0, 1.67f, 0);
    private float navMeshSurfaceAgentRadius = 1f;

    private int healthyAgentID = -1372625422;  // Make these in a Constants script? (recentlyHealedAgentID is used in HospitalBarrier.cs)
    private int infectedAgentID = -334000983;
    private int recentlyHealedAgentID = 1479372276;
    private int priorityInfectedAgentID = -1923039037;
    // private float hospitalEnterPosSideMax = 4.85f;

    public bool startsInfected;
    [SerializeField] bool isInfected;
    private bool isRecentlyInfected;

    public float infectionDeathDuration = 40f;
    public float healTime;

    [Header("Materials")]
    public Material healthyMaterial;
    public Material[] infectedMaterials;
    public Material deadMaterial;

    private InfectionCylinder infectionCylinderScript;
    private HospitalTile hospitalTileScript;

    internal float hospitalTileDistance;

    public float infectedSetPathTime = 1f;
    private float infectedSetPathTimer = -1f;
    // private CoroutineHandle handle;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        Debug.Log(agent.agentTypeID);
        // Debug.Log("ID: " + GetInstanceID());

        isInfected = startsInfected;
        isRecentlyInfected = startsInfected;

        infectionCylinderScript = GetComponentInChildren<InfectionCylinder>(true);
        hospitalTileScript = GameObject.Find("Hospital").GetComponent<HospitalTile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= minDistanceRelocation && !isInfected)  // Returns 0 when reached destination or when blocked, as close as it can get
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
        if (isOnHospitalTile(false))
        {
            Vector3 exitPos = transform.position;
            heal();
            Timing.RunCoroutine(CheckIfOffHospitalTile());
            isRecentlyInfected = false;  // Just in case
            Timing.RunCoroutine(HealingProcess());
            // Debug.Log("AFTER COROUTINE");
            /*
            exitPos.x = -exitPos.x;
            exitPos.z = -exitPos.z;
            agent.Warp(exitPos);
            */
        }
        if (isInfected)
        {
            bool pathPending = true;
            if (infectedSetPathTimer < 0)  // May want to change NavMeshAgent acceleration as well
            {
                pathPending = agent.SetDestination(hospitalTilePos);
                infectedSetPathTimer = infectedSetPathTime;
            }
            infectedSetPathTimer -= Time.deltaTime;
            // Debug.Log("GOING THERE: " + pathPending);
            // Debug.Log("REMAINING: " + agent.remainingDistance);
            hospitalTileDistance = agent.remainingDistance;
            if (isRecentlyInfected)
            {
                Timing.RunCoroutine(InfectionProcess(), "InfectionProcess");
                Timing.RunCoroutine(infectionCylinderScript.SinusoidalRadius(), "SinusoidalRadius " + GetInstanceID());
                GameManager.instance.infectedPathDistances[gameObject] = hospitalTileDistance;
                isRecentlyInfected = false;
            }
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

    bool isOnHospitalTile(bool isAddingTileOffset)
    {
        float halfTileSize = tileSize / 2;
        if (isAddingTileOffset)
        {
            halfTileSize += navMeshSurfaceAgentRadius;
        }
        bool isInXRange = transform.position.x > -halfTileSize && transform.position.x < halfTileSize;
        bool isInZRange = transform.position.z > -halfTileSize && transform.position.z < halfTileSize;
        if (isInXRange && isInZRange)
        {
            return true;
        }
        return false;
    }

    void heal()
    {
        if (agent.agentTypeID == priorityInfectedAgentID || agent.agentTypeID == infectedAgentID)  // 2nd check probably unnecessary
        {
            gameObject.tag = "Untagged";
            agent.agentTypeID = recentlyHealedAgentID;
            agent.SetDestination(hospitalTilePos);
            Timing.KillCoroutines("InfectionProcess");  // Kills in GameManager.cs for all infected
            Timing.KillCoroutines("SinusoidalRadius " + GetInstanceID());  // Kills in GameManager.cs for all infected
            infectionCylinderScript.gameObject.SetActive(false);
            gameObject.GetComponent<Renderer>().material = healthyMaterial;
        }
        Timing.RunCoroutine(hospitalTileScript.HospitalQueue());
    }

    IEnumerator<float> CheckIfOffHospitalTile()
    {
        while (isOnHospitalTile(true))
        {
            yield return Timing.WaitForOneFrame;
        }
        agent.agentTypeID = healthyAgentID;
    }

    IEnumerator<float> InfectionProcess()
    {
        gameObject.tag = "Infected";
        agent.agentTypeID = infectedAgentID;
        float individualStageTime = infectionDeathDuration / infectedMaterials.Length;
        for (int i = 0; i < infectedMaterials.Length; i++)
        {
            gameObject.GetComponent<Renderer>().material = infectedMaterials[i];
            yield return Timing.WaitForSeconds(individualStageTime);
        }
        // Timing.StopCoroutine(SetDestinationAsHospitalContinuously());
        gameObject.GetComponent<Renderer>().material = deadMaterial;
        agent.isStopped = true;
        GameManager.instance.GameOver();
    }

    IEnumerator<float> HealingProcess()
    {
        yield return Timing.WaitForSeconds(healTime);
        isInfected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isInfected && other.CompareTag("InfectionCylinder") && other.transform.parent.gameObject != gameObject)
        {
            isInfected = true;
            isRecentlyInfected = true;
            // Debug.Log("PERSON TO PERSON");
        }
    }
}