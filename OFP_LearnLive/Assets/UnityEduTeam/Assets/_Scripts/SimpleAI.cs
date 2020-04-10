using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SimpleAI : MonoBehaviour {
    [Header("Agent Field of View Properties")]
    public float viewRadius;
    public float viewAngle;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Space(5)]
    [Header("Agent Properties")]
    public float runSpeed;
    public float walkSpeed;
    public float patrolRadius;

    private NavMeshAgent agent;
    private Animator anim;

    private Transform playerTarget;

    private Vector3 currentDestination;

    private bool playerSeen;

    private enum State {Wandering, Chasing};
    private State currentState;


    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        currentDestination = RandomNavSphere(transform.position, patrolRadius, -1);
    }

    private void CheckState()
    {
        FindVisibleTargets();

        switch(currentState)
        {
            case State.Chasing:
                ChaseBehavior();
                break;

            default:
                WanderBehavior();
                break;

        }
    }

    void WanderBehavior()
    {
        anim.SetTrigger("walk");
        agent.speed = walkSpeed;

        float dist = agent.remainingDistance;

        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            currentDestination = RandomNavSphere(transform.position, patrolRadius, -1);
            agent.SetDestination(currentDestination);
        }

    }

    void ChaseBehavior()
    {
        if (playerTarget != null)
        {
            anim.SetTrigger("run");
            agent.speed = runSpeed;
            currentDestination = playerTarget.transform.position;
            agent.SetDestination(currentDestination);
        }
        else
        {
            playerSeen = false;
            currentState = State.Wandering;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

    #region Vision
    void FindVisibleTargets()
    {

        playerTarget = null;
        playerSeen = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return;
        }
        else
        {
            Debug.Log("Found Player");
        }

        foreach (GameObject player in players)
        {
            Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit))
            {
                float dstToTarget = Vector3.Distance(transform.position, player.transform.position);
                if (dstToTarget <= viewRadius)
                {
                    if (Vector3.Angle(transform.forward, dirToTarget) <= viewAngle / 2)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            playerSeen = true;
                            playerTarget = hit.transform;
                        }
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    #endregion

    // Update is called once per frame
    void Update () {
        CheckState();

        if (playerSeen)
        {
            currentState = State.Chasing;
        } else
        {
            currentState = State.Wandering;
        }
	}
}
