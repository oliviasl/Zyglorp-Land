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
            Cry,
            Patrol
        }
        
        [SerializeField] private Transform player;
        private HelmetHandler helmetHandler;
        private ChildState state = ChildState.Patrol;
        private int patrolIdx;
        private NavMeshAgent agent;
        private float viewConeAngle = 30f;
        private float viewConeRange = 15f;
      
        public void Start()
        {
            state = ChildState.Patrol;
            agent = GetComponent<NavMeshAgent>();
            helmetHandler = player.GetComponent<HelmetHandler>();
            patrolIdx = Random.Range(0, patrolPoints.Count);
            agent.SetDestination(patrolPoints[patrolIdx].position);
        }
        public void Abuse()
        {
            ChildState state = ChildState.Cry;
        }

        public void Console()
        {
            ChildState state = ChildState.Patrol;
        }

        public void Interact()
        {
            Abuse();
            // do animation stuff here
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
        private bool CheckPlayerMask()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized; //get the direction again lol

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // bascically just doing math to do damage instead of damage colliders
            // similar to hitscan i guess, where if they are within the range they take damage instead of lining up with cone that instatiates for instance

            if(angleToPlayer <= viewConeAngle / 2f 
               && distanceToPlayer <= viewConeRange
               && !helmetHandler.GetIsHelmetOn())
            {
                return true;
            }
        
            return false;
        }
    }
}