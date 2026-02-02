using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Manager
{
    public class Child : MonoBehaviour, IInteractable
    {
        [Header("Animation")]
        [SerializeField] private Animator kidAnim;

        public bool kicked;
        public enum ChildState
        {
            Patrol,
            Abused,
            Cry
        }
        public ChildState state = ChildState.Patrol;
        
        public bool abused;
        public float recoverTime = 4f;
        public float timeElapsed;
        
        private int patrolIdx;
        
        private Transform player;
        private HelmetHandler helmetHandler;

        [Header("NavMesh")]
        [SerializeField] private Vector3 walkPoint;
        [SerializeField] private bool walkPointSet;
        [SerializeField] private float walkPointRange;
        [SerializeField] private LayerMask whatIsGround;

        private NavMeshAgent agent;
        private float viewConeAngle = 30f;
        private float viewConeRange = 6f;
        private float minimumProximity = 2f;
      
        public void Start()
        {
            state = ChildState.Patrol;
            agent = GetComponent<NavMeshAgent>();
            player = FindFirstObjectByType<CharacterController>().GetComponent<Transform>();
            helmetHandler = player.GetComponent<HelmetHandler>();
            // Patrolling();
            SearchForPatrolPoint();
            
            if(AlienManager.Instance == null) Debug.LogError("AlienManager is null");
            AlienManager.Instance.children.Add(this);
        }

        public void Update()
        {
            CheckPlayerMask();
            switch (state)
            {
                case ChildState.Patrol:
                    Patrolling();
                    break;
                case ChildState.Abused:
                    Recover();
                    break;
                case ChildState.Cry:
                    break;
            }
        }
        
        private void HandleStateChange(ChildState newState)
        {
            state = newState;
            switch (newState)
            {
                case ChildState.Patrol:
                    abused = false;
                    // Patrolling();
                    SearchForPatrolPoint();
                    break;
                case ChildState.Abused:
                    abused = true;
                    agent.ResetPath();
                    walkPoint = transform.position;
                    AlienManager.Instance.AlertAbuse();
                    break;
                case ChildState.Cry:
                    abused = false;
                    agent.ResetPath();
                    AlienManager.Instance.AlertCry(transform.position);
                    break;
            }
        }

        private void SearchForPatrolPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            /* if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
             {
                 walkPointSet = true;
             }
            */

            if (Physics.Raycast(new Vector3(walkPoint.x, walkPoint.y + 5f, walkPoint.z), Vector3.down, 10f, whatIsGround))
            {
                walkPointSet |= true;
            }
        }

        public void Patrolling()
        {
            if (!walkPointSet)
            {
                SearchForPatrolPoint();
            }

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
                kidAnim.SetBool("IsWalking", true);
                //agent.SetDestination(new Vector3(0f, 0f, 0f));
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                kidAnim.SetBool("IsWalking", false);
            }
        }

        private void Recover()
        {
            timeElapsed  += Time.deltaTime;
            if (timeElapsed >= recoverTime)
            {
                timeElapsed = 0f;
                HandleStateChange(ChildState.Cry);
            }
        }
        public void Interact()
        {
            Abuse();
            // do animation stuff here
        }
        public void Abuse()
        {
            HandleStateChange(ChildState.Abused);
            kidAnim.SetTrigger("KidShoved");
            if (!kicked)
            {
                BombManager.instance.KickCounter();
                kicked = true;
            }
        }

        public void Tend()
        {
            HandleStateChange(ChildState.Patrol);
            kidAnim.SetTrigger("KidCalmed");
        }
        private void CheckPlayerMask()
        {
            if (state != ChildState.Patrol) return;
            Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if(angleToPlayer <= viewConeAngle / 2f 
               && distanceToPlayer <= viewConeRange
               && !helmetHandler.GetIsHelmetOn())
            {
                HandleStateChange(ChildState.Cry);
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
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.forward * minimumProximity));
        }
    }
}