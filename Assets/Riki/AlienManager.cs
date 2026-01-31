using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AlienManager : MonoBehaviour
{
    public List<Transform> patrolPoints;    
    public Vector3 targetPoint; 
    public float proximityRadius = 20f; 
    public float childCareTime = 5f;
    public enum ManagerState
    {
        Patrol,
        Tending,
        Chase,
        Abduct
    }

    private ManagerState state = ManagerState.Patrol;
    [SerializeField] private Collider tendingCollider;
    [SerializeField] private Transform player;
    private HelmetHandler helmetHandler;
    private int patrolIdx;
    private NavMeshAgent agent;
    private float viewConeAngle = 30f;
    private float viewConeRange = 15f;
    


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
                float dist = agent.remainingDistance;
                if (dist != Mathf.Infinity 
                    && agent.pathStatus == NavMeshPathStatus.PathComplete 
                    && agent.remainingDistance == 0)
                {
                    SetNewPatrolPoint();
                }
                break;
            // wait certain time and resume patrol
            case ManagerState.Tending:
                break;
            case ManagerState.Chase:
                agent.destination = player.position;
                float distToPlayer = agent.remainingDistance;
                if (distToPlayer != Mathf.Infinity 
                    && agent.pathStatus == NavMeshPathStatus.PathComplete 
                    && agent.remainingDistance == 0)
                {
                    HandleStateChange(ManagerState.Abduct);
                }
                break;
            case ManagerState.Abduct:
                break;
        }
    }
    
    private void HandleStateChange(ManagerState newState)
    {
        state = newState;
        switch (newState)
        {
            case ManagerState.Patrol:
                SetNewPatrolPoint();
                Debug.Log("changing state to patrol");
                break;
            case ManagerState.Tending:
                Debug.Log("changing state to tending");
                break;
            case ManagerState.Chase:
                agent.SetDestination(player.position);
                Debug.Log("changing state to chase");
                break;
            case ManagerState.Abduct:
                agent.isStopped = true;
                Debug.Log("changing state to abduct");
                break;
        }
    }
    
    // ignore patrol, go to nearest child and calm them down
    public IEnumerator HandleChildren()
    {
        state = ManagerState.Tending;
        tendingCollider.enabled = true;
        yield return new WaitForSeconds(childCareTime);
        tendingCollider.enabled = false;
        HandleStateChange(ManagerState.Patrol);
    }

    private void SetNewPatrolPoint()
    {
        int oldPatrolIdx = patrolIdx;
        while (patrolIdx ==  oldPatrolIdx)
        {
            patrolIdx = Random.Range(0, patrolPoints.Count);
        }
        agent.SetDestination(patrolPoints[patrolIdx].position);
        Debug.Log($"Set new dest to {patrolPoints[patrolIdx].position}");
    }
    private void CheckPlayerMask()
    {
        if (state == ManagerState.Chase || state == ManagerState.Abduct) return;
        
        Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // bascically just doing math to do damage instead of damage colliders
        // similar to hitscan i guess, where if they are within the range they take damage instead of lining up with cone that instatiates for instance

        if(angleToPlayer <= viewConeAngle / 2f 
           && distanceToPlayer <= viewConeRange
           && !helmetHandler.GetIsHelmetOn())
        {
            HandleStateChange(ManagerState.Chase);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewConeAngle); //flamethrowerRange

        //cone below
        Vector3 leftBoudnry = Quaternion.Euler(0, -viewConeAngle / 2f, 0) * transform.forward * viewConeRange;
        Vector3 rightBoudnry = Quaternion.Euler(0, viewConeAngle / 2f, 0) * transform.forward * viewConeRange;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoudnry);
        Gizmos.DrawLine(transform.position, transform.position + rightBoudnry);
    }
}
