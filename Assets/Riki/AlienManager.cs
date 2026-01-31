using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AlienManager : MonoBehaviour
{
    public List<Child> children; 
    public List<Transform> patrolPoints;
    public enum ManagerState
    {
        Patrol,
        Tending,
        Chase,
        Abduct
    }

    private ManagerState state = ManagerState.Patrol;
    private int patrolIdx;
    
    [SerializeField] private Transform player;
    private HelmetHandler helmetHandler;
    
    private NavMeshAgent agent;
    private float viewConeAngle = 45f;
    private float viewConeRange = 20f;
    private float minimumProximity = 1f;
    
    private bool findingChild;
    private float childCareTime = 5f;
    private float timeTended;


    public void Start()
    {
        state = ManagerState.Patrol;
        helmetHandler = player.GetComponent<HelmetHandler>();
        agent = GetComponent<NavMeshAgent>();
        patrolIdx = Random.Range(0, patrolPoints.Count);
        agent.SetDestination(patrolPoints[patrolIdx].position);
    }

    public void Update()
    {
        CheckPlayerMask();
        switch (state)
        {
            case ManagerState.Patrol:
                CheckRemainingDistance();
                break;
            // wait certain time and resume patrol
            case ManagerState.Tending:
                TendChildren();
                break;
            case ManagerState.Chase:
                Chase();
                break;
            case ManagerState.Abduct:
                Abduct();
                break;
        }
    }
    
    public void HandleStateChange(ManagerState newState)
    {
        state = newState;
        switch (newState)
        {
            case ManagerState.Patrol:
                SetNewPatrolPoint();
                Debug.Log("changing state to patrol");
                break;
            case ManagerState.Tending:
                agent.isStopped = true;
                agent.ResetPath();
                findingChild = false;
                Debug.Log("changing state to tending");
                break;
            case ManagerState.Chase:
                findingChild = false;
                agent.SetDestination(player.position);
                Debug.Log("changing state to chase");
                break;
            case ManagerState.Abduct:
                agent.isStopped = true;
                agent.ResetPath();
                Debug.Log("changing state to abduct");
                break;
        }
    }

    #region State Behavior
    private void CheckRemainingDistance()
    {
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (findingChild && agent.remainingDistance <= minimumProximity)
            {
                HandleStateChange(ManagerState.Tending);
            }
            else if(agent.remainingDistance == 0)
            {
                SetNewPatrolPoint();
            }
        }
    }

    private void TendChildren()
    {
        timeTended += Time.deltaTime;
        if (timeTended >= childCareTime)
        {
            timeTended = 0f;
            HandleChildren();
        }
    }

    private void Chase()
    {
        agent.destination = player.position;
        float distToPlayer = agent.remainingDistance;
        if (distToPlayer != Mathf.Infinity 
            && agent.pathStatus == NavMeshPathStatus.PathComplete 
            && agent.remainingDistance <= minimumProximity)
        {
            HandleStateChange(ManagerState.Abduct);
        }
    }

    private void Abduct()
    {
        
    }
    
    #endregion

    public void AlertTaskFail()
    {
        HandleStateChange(ManagerState.Chase);
    }
    
    public void AlertAbuse()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // player is in view
        if (angleToPlayer <= viewConeAngle / 2f && distanceToPlayer <= viewConeRange)
        {
            HandleStateChange(ManagerState.Chase);
        }
    }
    
    public void AlertCry(Vector3 pos)
    {
        Debug.Log("baby alert");
        agent.ResetPath();
        agent.SetDestination(pos);
        findingChild = true;
    }
    
    #region Helper Functions
    private void HandleChildren()
    {
        foreach (var child in children)
        {
            child.Tend();
        }
        HandleStateChange(ManagerState.Patrol);
    }

    private void SetNewPatrolPoint()
    {
        int oldPatrolIdx = patrolIdx;
        while (patrolIdx == oldPatrolIdx)
        {
            patrolIdx = Random.Range(0, patrolPoints.Count);
        }
        agent.SetDestination(patrolPoints[patrolIdx].position);
    }
    private void CheckPlayerMask()
    {
        if (state == ManagerState.Chase || state == ManagerState.Abduct) return;
        
        Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // player is in view
        if(angleToPlayer <= viewConeAngle / 2f && distanceToPlayer <= viewConeRange && !helmetHandler.GetIsHelmetOn())
        {
            HandleStateChange(ManagerState.Chase);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewConeAngle);

        //cone below
        Vector3 leftBoudnry = Quaternion.Euler(0, -viewConeAngle / 2f, 0) * transform.forward * viewConeRange;
        Vector3 rightBoudnry = Quaternion.Euler(0, viewConeAngle / 2f, 0) * transform.forward * viewConeRange;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoudnry);
        Gizmos.DrawLine(transform.position, transform.position + rightBoudnry);
    }
    #endregion
}
