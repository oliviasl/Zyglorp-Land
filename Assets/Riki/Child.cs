using UnityEngine;

namespace Manager
{
    public class Child : MonoBehaviour
    {
        public enum ChildState
        {
            Cry,
            Patrol
        }
        
        private ChildState _state = ChildState.Patrol;
        
        public void Abuse()
        {
            ChildState state = ChildState.Cry;
        }

        public void Console()
        {
            ChildState state = ChildState.Patrol;
        }
    }
}