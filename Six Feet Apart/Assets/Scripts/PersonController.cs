// using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using MEC;
using System.Threading;
using System.IO;
// using System;
// using NUnit.Framework.Internal;
// using NUnit.Framework;
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
    private int farInfectedAgentID = -902729914;
    // private float hospitalEnterPosSideMax = 4.85f;

    public bool startsInfected;
    [SerializeField] bool isInfected;
    private bool isRecentlyInfected;

    public float infectionDeathDuration = 40f;
    public float healTime;  // Value also exists in HealProgressBar.cs

    [Header("Materials")]
    public Material healthyMaterial;
    public Material[] infectedMaterials;
    public Material deadMaterial;

    private InfectionCylinder infectionCylinderScript;
    private HospitalTile hospitalTileScript;

    internal float hospitalTileDistance;

    public float infectedSetPathTime = 1f;
    private float infectedSetPathTimer = -1f;

    private bool hasStartedHealing = false;

    private float healthyAcceleration = 15;
    private float infectedAcceleration = 400;
    // private CoroutineHandle handle;

    public int hospitalPathMode = 2;
    Vector3[][] pathModeDestinations = new Vector3[3][];
    private NavMeshAgent tempAgent;
    private float tempAgentYPos = -4.41f;

    private bool isNearHospital = false;  // Unused
    private float farInfectedHospitalBorderWidth = 1.5f;  // Try 2f if range is too close
    private float wallYPos = 1f;

    // private bool isGameActivePreviousFrame = true;
    private bool wasAgentStopped = false;

    public float priorityInfectedHospitalReachTime = 0.2f;
    private float priorityInfectedHospitalReachTimer;

    // public float speedThreshold = 1f;
    private int defaultAvoidancePriority = 50;
    private int highestAvoidancePriority = 25;
    private int lowestAvoidancePriority = 75;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        // Debug.Log(agent.agentTypeID);
        // Debug.Log("ID: " + GetInstanceID());

        isInfected = startsInfected;
        isRecentlyInfected = startsInfected;

        infectionCylinderScript = GetComponentInChildren<InfectionCylinder>(true);
        hospitalTileScript = GameObject.Find("Hospital").GetComponent<HospitalTile>();

        GeneratePathModeDestinations(true);
        tempAgent = GameObject.Find("Temp Pathing Agent").GetComponent<NavMeshAgent>();

        priorityInfectedHospitalReachTimer = priorityInfectedHospitalReachTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameActive)
        {
            if (agent.isStopped == true && GameManager.instance.isGameActivePreviousFrame)
            {
                wasAgentStopped = true;  // For later use with revives
            }
            agent.isStopped = true;  // Occasionally Person 1 and Person 2 (and maybe others as well) are not stopped with GameManager.instance.isGameActivePreviousFrame added in the largest if-block
            // isGameActivePreviousFrame = false;
            // Debug.Log(gameObject.name + " STOPPED");
        }
        /*
        if (GameManager.instance.isGameActive && !isGameActivePreviousFrame)
        {
            agent.isStopped = false;
            isGameActivePreviousFrame = true;
            Debug.Log("Here 2");
        }
        */
        if (agent.remainingDistance <= minDistanceRelocation && !isInfected)  // Returns 0 when reached destination or when blocked, as close as it can get
        {
            float xPos = Random.Range(-xPosRange, xPosRange);
            float zPos = Random.Range(-zPosRange, zPosRange);
            Vector3 newLocation = new Vector3(xPos, yPos, zPos);
            if (CalculateNewPath(newLocation, false))
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
        if (IsOnHospitalTile(0) && !hasStartedHealing)
        {
            Vector3 exitPos = transform.position;
            Heal();
            Debug.Log("HEALING " + gameObject.name);
            Timing.RunCoroutine(hospitalTileScript.HospitalQueue());  // Moved from outside if-block in Heal();
            HealProgressBar.isNewlyOccupied = true;
            Timing.RunCoroutine(CheckIfOffHospitalTile());
            isRecentlyInfected = false;  // Just in case
            Timing.RunCoroutine(HealingProcess());
            hasStartedHealing = true;
            // Debug.Log("AFTER COROUTINE");
            /*
            exitPos.x = -exitPos.x;
            exitPos.z = -exitPos.z;
            agent.Warp(exitPos);
            */
        }
        else if (!IsOnHospitalTile(0))
        {
            hasStartedHealing = false;
        }
        if (isInfected && agent.agentTypeID != recentlyHealedAgentID && agent.agentTypeID != priorityInfectedAgentID)  // Moved priorityInfectedAgentID here
        {
            bool pathPending = true;
            // bool notPriorityNorRecentlyHealed = agent.agentTypeID != priorityInfectedAgentID && agent.agentTypeID != recentlyHealedAgentID;  // Put recentlyHealed check in main if-condition
            if (agent.remainingDistance != Mathf.Infinity && IsOnHospitalTile(farInfectedHospitalBorderWidth))  // When path is a straight line to hospital and close enough
            {
                if (agent.agentTypeID == farInfectedAgentID)
                {
                    agent.agentTypeID = infectedAgentID;
                    agent.SetDestination(hospitalTilePos);
                }
                else
                {
                    agent.agentTypeID = infectedAgentID;  // Fixed v0.7 Alpha 1 Bug #1?
                }
            }
            if (infectedSetPathTimer < 0)  // Changed NavMeshAgent acceleration to counteract path recalculation navigation pauses
            {
                if (agent.agentTypeID == farInfectedAgentID)
                {
                    agent.SetDestination(hospitalTilePos);
                }
                else
                {
                    Vector3 shiftedOrigin = new Vector3(transform.position.x, wallYPos, transform.position.z);
                    Vector3 hospitalDirection = hospitalTilePos - transform.position;
                    float hospitalDistance = Vector3.Distance(transform.position, hospitalTilePos);
                    int layerMask = 1 << 9;  // Ray cast only against colliders in Layer 9: Walls
                    bool hasLineOfSightWithHospital = !Physics.Raycast(shiftedOrigin, hospitalDirection, hospitalDistance, layerMask);
                    Debug.DrawRay(shiftedOrigin, hospitalDirection, UnityEngine.Color.red, 2f);
                    // Debug.Log("Line of sight with Hospital: " + hasLineOfSightWithHospital);
                    if (CalculateNewPath(hospitalTilePos, true) && !hasLineOfSightWithHospital)  // This if-block seems to never be run, but should when path to hospital found and agent type is infectedSpheroid
                    {
                        agent.agentTypeID = farInfectedAgentID;
                        // agent.SetDestination(hospitalTilePos);  // Enable?
                    }
                    else
                    {
                        float minPathLength = float.MaxValue;
                        Vector3 minPathLocation = hospitalTilePos;  // Defaults here if no complete paths
                        foreach (Vector3 location in pathModeDestinations[hospitalPathMode])
                        {
                            float pathLength = GetPathLength(location, true);
                            // Debug.Log(location + ": " + pathLength);
                            if (pathLength < minPathLength)
                            {
                                minPathLength = pathLength;
                                minPathLocation = location;
                            }
                        }
                        pathPending = agent.SetDestination(minPathLocation);
                    }
                }
                infectedSetPathTimer = infectedSetPathTime;
            }
            infectedSetPathTimer -= Time.deltaTime;
            // Debug.Log("GOING THERE: " + pathPending);
            // Debug.Log("REMAINING: " + agent.remainingDistance);
            hospitalTileDistance = agent.remainingDistance;
            if (isRecentlyInfected)
            {
                Timing.RunCoroutine(InfectionProcess(), "InfectionProcess " + GetInstanceID());
                Timing.RunCoroutine(infectionCylinderScript.SinusoidalRadius(), "SinusoidalRadius " + GetInstanceID());
                // GameManager.instance.infectedPathDistances[gameObject] = hospitalTileDistance;
                isRecentlyInfected = false;
            }
        }
        else if (agent.agentTypeID == priorityInfectedAgentID)
        {
            priorityInfectedHospitalReachTimer -= Time.deltaTime;
            if (priorityInfectedHospitalReachTimer < 0)
            {
                agent.agentTypeID = infectedAgentID;
                HospitalTile.isOccupied = false;
                Debug.Log("UNOCCUPIED 2 BY " + gameObject.name);
                priorityInfectedHospitalReachTimer = priorityInfectedHospitalReachTime;
            }
        }
    }
    bool CalculateNewPath(Vector3 targetPos, bool usePriorityInfectedAgent)
    {
        if (usePriorityInfectedAgent)
        {
            Vector3 warpPosition = new Vector3(transform.position.x, tempAgentYPos, transform.position.z);
            tempAgent.Warp(warpPosition);  // If decided to not warp, tempAgent will stay at Hospital at all times
            // Debug.Log("Temp Location: " + tempAgent.transform.position);
            Vector3 newTargetPos = new Vector3(targetPos.x, tempAgentYPos, targetPos.z);
            tempAgent.CalculatePath(newTargetPos, navMeshPath);
        }
        else
        {
            agent.CalculatePath(targetPos, navMeshPath);
        }
        // Debug.Log("New path calculated");
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        return true;
    }

    public float GetPathLength(Vector3 targetPos, bool usePriorityInfectedAgent)
    {
        float pathLength = 0;
        if (CalculateNewPath(targetPos, usePriorityInfectedAgent))  // Resulting path stored in navMeshPath
        {
            for (int i = 1; i < navMeshPath.corners.Length; i++)
            {
                pathLength += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }
        else
        {
            return float.MaxValue;
        }
        return pathLength;
    }

    bool IsOnHospitalTile(float extraBorderWidth)
    {
        float halfTileSize = tileSize / 2;
        halfTileSize += extraBorderWidth;
        bool isInXRange = transform.position.x > -halfTileSize && transform.position.x < halfTileSize;
        bool isInZRange = transform.position.z > -halfTileSize && transform.position.z < halfTileSize;
        if (isInXRange && isInZRange)
        {
            return true;
        }
        return false;
    }

    void Heal()
    {
        if (agent.agentTypeID == priorityInfectedAgentID || agent.agentTypeID == infectedAgentID)  // 2nd check probably unnecessary
        {
            gameObject.tag = "Untagged";
            agent.agentTypeID = recentlyHealedAgentID;
            agent.acceleration = healthyAcceleration;
            agent.SetDestination(hospitalTilePos);
            Timing.KillCoroutines("InfectionProcess " + GetInstanceID());  // Works fine without GetInstanceID()?
            Timing.KillCoroutines("SinusoidalRadius " + GetInstanceID());
            infectionCylinderScript.gameObject.SetActive(false);
            gameObject.GetComponent<Renderer>().material = healthyMaterial;
        }
    }

    IEnumerator<float> CheckIfOffHospitalTile()
    {
        while (IsOnHospitalTile(navMeshSurfaceAgentRadius))
        {
            yield return Timing.WaitForOneFrame;
        }
        agent.agentTypeID = healthyAgentID;
        HospitalTile.isOccupied = false;
        Debug.Log("UNOCCUPIED " + gameObject.name);
    }

    IEnumerator<float> InfectionProcess()
    {
        gameObject.tag = "Infected";
        if (isNearHospital)
        {
            agent.agentTypeID = infectedAgentID;
        }
        else
        {
            agent.agentTypeID = farInfectedAgentID;
        }
        agent.acceleration = infectedAcceleration;
        float individualStageTime = infectionDeathDuration / infectedMaterials.Length;
        for (int i = 0; i < infectedMaterials.Length; i++)
        {
            gameObject.GetComponent<Renderer>().material = infectedMaterials[i];
            yield return Timing.WaitForSeconds(individualStageTime);
        }
        // Timing.StopCoroutine(SetDestinationAsHospitalContinuously());
        gameObject.GetComponent<Renderer>().material = deadMaterial;
        Timing.KillCoroutines("SinusoidalRadius " + GetInstanceID());
        infectionCylinderScript.gameObject.SetActive(false);
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
        if (!isInfected && other.CompareTag("InfectionCylinder") && other.transform.parent.gameObject != gameObject && !IsOnHospitalTile(0))  // May remove Hospital immunity
        {
            isInfected = true;
            isRecentlyInfected = true;
            // Debug.Log("PERSON TO PERSON");
        }
        else if (other.CompareTag("AvoidanceCylinder") && other.transform.parent.gameObject != gameObject && !isInfected)
        {
            agent.avoidancePriority = Random.Range(highestAvoidancePriority, lowestAvoidancePriority);
        }
        else if (other.CompareTag("NearHospital"))  // isNearHospital and "NearHospital" tag eems to be unused now
        {
            isNearHospital = true;
            if (agent.agentTypeID == farInfectedAgentID)
            {
                agent.agentTypeID = infectedAgentID;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NearHospital"))
        {
            isNearHospital = false;
        }
        else if (other.CompareTag("AvoidanceCylinder"))
        {
            agent.avoidancePriority = defaultAvoidancePriority;
        }
    }

    void GeneratePathModeDestinations(bool isSubtractingHospitalWalls)
    {
        float val1;
        if (isSubtractingHospitalWalls)
        {
            val1 = 6.17f;  // Distance from center of Hospital to InfectedSpheroid's NavMesh without wall (minus 0.03 to collide with Hospital barriers)
        }
        else
        {
            val1 = 6.5f;  // Distance from center of tile and through a wall of adjacent tile
        }
        float val2 = 3.5f;  // Distance from center of tile and touching wall on same tile
        pathModeDestinations[0] = new Vector3[]{};
        pathModeDestinations[1] = new Vector3[4];
        pathModeDestinations[2] = new Vector3[12];
        for (int i = 0; i < 4; i++)
        {
            float newVal1 = val1;
            if (i % 2 == 1)
            {
                newVal1 *= -1;
            }
            if (i < 2)
            {
                pathModeDestinations[1][i] = new Vector3(newVal1, yPos, 0);
                pathModeDestinations[2][i] = new Vector3(newVal1, yPos, 0);
            }
            else
            {
                pathModeDestinations[1][i] = new Vector3(0, yPos, newVal1);
                pathModeDestinations[2][i] = new Vector3(0, yPos, newVal1);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            float newVal1 = val1;
            float newVal2 = val2;
            if (i % 2 == 1)
            {
                newVal1 *= -1;
            }
            if (i % 4 >= 2)
            {
                newVal2 *= -1;
            }
            if (i < 4)
            {
                pathModeDestinations[2][i + 4] = new Vector3(newVal1, yPos, newVal2);
            }
            else
            {
                pathModeDestinations[2][i + 4] = new Vector3(newVal2, yPos, newVal1);
            }
        }
    }
}