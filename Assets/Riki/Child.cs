using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Manager
{
    public class Child : MonoBehaviour, IInteractable
    {
        public List<Transform> patrolPoints; 
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
        
        [SerializeField] private AlienManager manager;
        [SerializeField] private Transform player;
        private HelmetHandler helmetHandler;
        
        
        private NavMeshAgent agent;
        private float viewConeAngle = 30f;
        private float viewConeRange = 10f;
        private float minimumProximity = 5f;
      
        public void Start()
        {
            state = ChildState.Patrol;
            agent = GetComponent<NavMeshAgent>();
            helmetHandler = player.GetComponent<HelmetHandler>();
            patrolIdx = Random.Range(0, patrolPoints.Count);
            agent.SetDestination(patrolPoints[patrolIdx].position);
            manager.children.Add(this);
        }

        public void Update()
        {
            CheckPlayerMask();
            switch (state)
            {
                case ChildState.Patrol:
                    CheckDistance();
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
                    SetNewPatrolPoint();
                    break;
                case ChildState.Abused:
                    abused = true;
                    manager.AlertAbuse();
                    break;
                case ChildState.Cry:
                    abused = false;
                    agent.isStopped = true;
                    agent.ResetPath();
                    manager.AlertCry(transform.position);
                    break;
            }
        }

        private void CheckDistance()
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity 
                && agent.pathStatus == NavMeshPathStatus.PathComplete 
                && agent.remainingDistance <= minimumProximity)
            {
                SetNewPatrolPoint();
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
        }

        public void Tend()
        {
            HandleStateChange(ChildState.Patrol);
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
            if (state == ChildState.Cry) return;
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewConeAngle);

            //cone below
            Vector3 leftBoudnry = Quaternion.Euler(0, -viewConeAngle / 2f, 0) * transform.forward * viewConeRange;
            Vector3 rightBoudnry = Quaternion.Euler(0, viewConeAngle / 2f, 0) * transform.forward * viewConeRange;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + leftBoudnry);
            Gizmos.DrawLine(transform.position, transform.position + rightBoudnry);
        }
    }
}