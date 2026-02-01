using System.Collections.Generic;
using Manager;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AlienManager : MonoBehaviour
{
    // i am SO sorry i just put everythong here i deserve death by firiing squad
    //                                                  - chopped chungus chud
    public static AlienManager Instance { get; private set; }
    public List<Child> children; 
    public List<Transform> patrolPoints;
    public enum ManagerState
    {
        Patrol,
        Tending,
        Chase,
        Abduct
    }

    [SerializeField] private Transform saucer;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Canvas fadeScreen;
    private FirstPersonController controller;
    
    private ManagerState state = ManagerState.Patrol;
    private int patrolIdx;
    
    private Transform player;
    private HelmetHandler helmetHandler;
    
    private NavMeshAgent agent;
    private float viewConeAngle = 65f;
    private float viewConeRange = 12f;
    private float minimumProximity = 1f;
    
    private float chaseSpeedBoost = 1.75f;
    
    private float abductTime = 5f;
    private float abductTimeElapsed = 0f;
    private float abductSpeed = 2f;
    
    private bool findingChild;
    private float childCareTime = 5f;
    private float timeTended;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        state = ManagerState.Patrol;
        player = FindFirstObjectByType<CharacterController>().GetComponent<Transform>();
        helmetHandler = player.GetComponent<HelmetHandler>();
        agent = GetComponent<NavMeshAgent>();
        patrolIdx = Random.Range(0, patrolPoints.Count);
        agent.SetDestination(patrolPoints[patrolIdx].position);
        controller = player.GetComponent<FirstPersonController>();
    }

    public void Update()
    {
        CheckPlayerMask();
        switch (state)
        {
            case ManagerState.Patrol:
                CheckRemainingDistance();
                break;
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
                agent.ResetPath();
                findingChild = false;
                Debug.Log("changing state to tending");
                break;
            case ManagerState.Chase:
                findingChild = false;
                agent.ResetPath();
                agent.SetDestination(player.position);
                Debug.Log("changing state to chase");
                break;
            // THE "ON STATE CHANGE" ABDUCT
            case ManagerState.Abduct:
                agent.ResetPath();
                agent.SetDestination(player.position + Camera.main.transform.forward * 2f);
                controller.EnableMovement(false);
                helmetHandler.abduction = true;
                saucer.transform.position = player.position + Vector3.up * 20f;
                abductTimeElapsed = 0f;
                helmetHandler.HideMaskScreen();
                GetComponent<Collider>().enabled = false;
                Debug.Log("changing state to abduct");
                break;
        }
    }

    #region State Behavior
    private void CheckRemainingDistance()
    {
        if (findingChild) 
        {
            Vector3 directionToChild = (agent.pathEndPosition - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToChild);
            float distanceToChild = Vector3.Distance(transform.position, agent.pathEndPosition);
            
            if(angleToPlayer <= viewConeAngle / 2f && distanceToChild <= viewConeRange/2 && agent.remainingDistance <= minimumProximity * 3f)
            {
                Debug.Log($"tending child at dist {distanceToChild}");
                HandleStateChange(ManagerState.Tending);
            }
        }
        else
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
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
        agent.SetDestination(player.position);
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer != Mathf.Infinity 
            && agent.pathStatus == NavMeshPathStatus.PathComplete 
            && distToPlayer <= minimumProximity)
        {
            Debug.Log($"got player at dist {distToPlayer}");
            HandleStateChange(ManagerState.Abduct);
        }
    }

    
    // THE "UPDATE" FUNCTION FOR ABDUCT
    private void Abduct()
    {
        abductTimeElapsed += Time.deltaTime;
        player.transform.position += Vector3.up * (abductSpeed * controller.Gravity * Time.deltaTime);
        if (abductTimeElapsed >= abductTime)
        {
            fadeScreen.enabled = true;
            if (abductTimeElapsed >= abductTime * 2)
            {
                Reset();
            }
        }
    }
    
    #endregion

    public void AlertTaskFail()
    {
        HandleStateChange(ManagerState.Chase);
    }
    
    public void AlertAbuse()
    {
        Debug.Log("abuse alert");
        if (state == ManagerState.Abduct || state == ManagerState.Chase) return;
        Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // player is in view
        if (angleToPlayer <= viewConeAngle / 2f && distanceToPlayer <= viewConeRange)
        {
            Debug.Log($"found player at dist {distanceToPlayer}");
            HandleStateChange(ManagerState.Chase);
        }
    }
    
    public void AlertCry(Vector3 pos)
    {
        Debug.Log("baby alert");
        if (state == ManagerState.Chase || state == ManagerState.Abduct) return;
        agent.ResetPath();
        agent.SetDestination(pos);
        findingChild = true;
    }

    // THE STUFF THAT HAPPENS WHEN PLAYER RESETS
    public void Reset()
    {
        HandleStateChange(ManagerState.Patrol);
        saucer.transform.position = new Vector3(100f, 100f, 100f);
        controller.EnableMovement(false);
        abductTimeElapsed = 0f;
        player.transform.position = spawnPoint;
        fadeScreen.enabled = false;
        helmetHandler.abduction = false;
        helmetHandler.ShowMaskScreen();
        GetComponent<Collider>().enabled = true;
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
        Debug.Log("setting new patrol point");
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
            Debug.Log($"found player at dist {distanceToPlayer}");
            HandleStateChange(ManagerState.Chase);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        //cone below
        Vector3 leftBoudnry = Quaternion.Euler(0, -viewConeAngle / 2f, 0) * transform.forward * viewConeRange;
        Vector3 rightBoudnry = Quaternion.Euler(0, viewConeAngle / 2f, 0) * transform.forward * viewConeRange;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoudnry);
        Gizmos.DrawLine(transform.position, transform.position + rightBoudnry);
    }
    #endregion
}
