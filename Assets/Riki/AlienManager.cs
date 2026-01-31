using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AlienManager : MonoBehaviour
{
    public enum ManagerState
    {
        Patrol,
        Tending,
        Chase,
        Abduct
    }

    private ManagerState state = ManagerState.Patrol;
    public List<Transform> patrolPoints;
    private int patrolIdx;
    private NavMeshAgent agent;
    public Vector3 targetPoint;
    public float proximityRadius = 20f;
    public float childCareTime = 5f;

    public void Start()
    {
        state = ManagerState.Patrol;
        agent = GetComponent<NavMeshAgent>();
        patrolIdx = Random.Range(0, patrolPoints.Count);
        agent.SetDestination(patrolPoints[patrolIdx].position);
    }

    public void Update()
    {
        switch (state)
        {
            // go to this place
            case ManagerState.Patrol:
                if (agent.isStopped)
                {
                    SetNewPatrolPoint();
                }
                break;
            // wait certain time and resume patrol
            case ManagerState.Tending:
                break;
            case ManagerState.Chase:
                if (agent.isStopped)
                {
                    state = ManagerState.Abduct;
                }
                break;
            case ManagerState.Abduct:
                break;
        }
    }
    
    // ignore patrol start moving towards player
    public void StartChase(Vector3 target)
    {
        state = ManagerState.Chase;
        targetPoint = target;
        agent.SetDestination(target);
    }

    public void Abduct()
    {
        state = ManagerState.Abduct;
        // yell at player
    }
    
    // ignore patrol, go to nearest child and calm them down
    public IEnumerator HandleChildren()
    {
        state = ManagerState.Tending;
        yield return new WaitForSeconds(childCareTime);
        state = ManagerState.Patrol;
        SetNewPatrolPoint();
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
}
