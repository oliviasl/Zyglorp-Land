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
    [Header("Animation")]
    [SerializeField] private Animator managerAnim;


    public static AlienManager Instance { get; private set; }
    public List<Child> children;
    public enum ManagerState
    {
        Patrol,
        Tending,
        Chase,
        Abduct
    }

    public ManagerState state = ManagerState.Patrol;
    
    [SerializeField] private Transform saucer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas fadeScreen;
    
    private FirstPersonController controller;
    private HelmetHandler helmetHandler;
    private Transform player;
    
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    [SerializeField] private LayerMask whatIsGround;
        
    private NavMeshAgent agent;
    private float viewConeAngle = 65f;
    private float viewConeRange = 12f;
    private float minimumProximity = 1f;
    
    private float speedBoost = 1f;
    
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
        controller = player.GetComponent<FirstPersonController>();
        managerAnim.SetBool("isWalking", true);
        SetNewPatrolPoint();
    }

    public void Update()
    {
        CheckPlayerMask();
        switch (state)
        {
            case ManagerState.Patrol:
                Patrol();
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
                managerAnim.SetTrigger("IsCalming");
                agent.speed /= speedBoost;
                agent.ResetPath();
                findingChild = false;
                Debug.Log("changing state to tending");
                break;
            case ManagerState.Chase:
                if (!findingChild)
                {
                    agent.speed *= speedBoost;
                }
                findingChild = false;
                agent.ResetPath();
                agent.SetDestination(player.position);
                helmetHandler.PlayMusic(helmetHandler.GetIsHelmetOn());
                Debug.Log("changing state to chase");
                break;
            // THE "ON STATE CHANGE" ABDUCT
            case ManagerState.Abduct:
                managerAnim.SetTrigger("Scolding");
                agent.speed /= speedBoost;
                agent.ResetPath();
                walkPoint = transform.position;
                // agent.SetDestination(player.position + Camera.main.transform.forward * 2f);
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
    private void Patrol()
    {
        if (!walkPointSet)
        {
            managerAnim.SetBool("IsWalking", false);
            SetNewPatrolPoint();
        }

        if (walkPointSet)
        {
            
            agent.SetDestination(walkPoint);
            managerAnim.SetBool("IsWalking", true);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (findingChild && distanceToWalkPoint.magnitude < 2f)
        {
            Debug.Log($"tending child at dist {distanceToWalkPoint}");
            HandleStateChange(ManagerState.Tending);
        }
        
        if (distanceToWalkPoint.magnitude < 1f)
        {
            
            walkPointSet = false;
            managerAnim.SetBool("IsWalking", false);
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
        managerAnim.SetTrigger("IsChasing");

        agent.SetDestination(player.position);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1.15f)
        {
            Debug.Log($"got player at dist {distanceToWalkPoint}");
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
        walkPoint = pos;
        agent.ResetPath();
        agent.SetDestination(pos);
        findingChild = true;
        agent.speed *= speedBoost;
    }

    // THE STUFF THAT HAPPENS WHEN PLAYER RESETS
    public void Reset()
    {
        managerAnim.SetTrigger("DoneScolding");
        HandleStateChange(ManagerState.Patrol);
        saucer.transform.position = new Vector3(100f, 100f, 100f);
        controller.EnableMovement(false);
        abductTimeElapsed = 0f;
        player.transform.position = spawnPoint.transform.position;
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
        managerAnim.SetTrigger("DoneCalming");
        HandleStateChange(ManagerState.Patrol);
    }

    private void SetNewPatrolPoint()
    {
        // Debug.Log("setting new patrol point");
        
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //managerAnim.SetBool("isWalking", true);

        if (Physics.Raycast(new Vector3(walkPoint.x, walkPoint.y + 5f, walkPoint.z), Vector3.down, 10f, whatIsGround))
        {
            walkPointSet |= true;
            
        }
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
