// Mover.cs file stands for arrangin movement feature for Jaim Game.

// Adding namespaces that we keep in safe 
using JAIM.Core;
using JAIM.Saving;
using UnityEngine;
using UnityEngine.AI;
using JAIM.Attributes;

namespace JAIM.Movement // this namespace holds attributes about movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable // Monobehaviour is the base class for all created component in unity
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent; //navmesh is a navigation system, it handles the pathfinding feature
        Health health;

        private void Awake() // awake works only once when it called
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); // defining navmeshagent in the awake() method
            health = GetComponent<Health>(); // defining health in the awake() method
        }

        void Update() // works once in every frame
        {
            navMeshAgent.enabled = !health.IsDead(); 
            UpdateAnimator(); // updates the animator in every frame
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) // this block of code arranges starting the moving action
        {
            GetComponent<ActionScheduler>().StartAction(this); // awakes the action scheduler
            MoveTo(destination, speedFraction); // takes destination and speed as parameter 
        }

        public bool CanMoveTo(Vector3 destination) // this block of code checks if player can go to the targeted point
        {
            NavMeshPath path = new NavMeshPath(); // navmesh stands for pathfinding
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path); // takes some parameters and defines selected point has a path
            if (!hasPath) return false; // if there is no path then return false
            if (path.status != NavMeshPathStatus.PathComplete) return false; // if there is not a complete path then return false
            if (GetPathLength(path) > maxNavPathLength) return false; // if the path is bigger than max allowed path then return false
            return true; // else return true and player can go to this point
        }
       
        
        public void MoveTo(Vector3 destination, float speedFraction) // speciying the destination, it takes destionation and speed parameters
        {
            navMeshAgent.destination = destination; // defining navmesh's destination
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); // defining navmesh's speed
            navMeshAgent.isStopped = false; // makes navmesh continue to work
        }

        public void Cancel() // canceling the process
        {
            navMeshAgent.isStopped = true; // makes navmesh stopped
        }

        private void UpdateAnimator() // this block of code updates the animator, it called in update() method, so this method works once in every frame
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        private float GetPathLength(NavMeshPath path) // this block of code gets the total path length
        {
            float total = 0; // in the beginning total is 0
            if (path.corners.Length < 2) return total; // if path's lenght smaller than 2 return total
            for (int i = 0; i < path.corners.Length - 1; i++) // finds the amount of path length
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total; // return the path's length
        }

        public object CaptureState() // capturing the current state
        {
            return new SerializableVector3(transform.position); // captures current position
        }

        public void RestoreState(object state) // restoring the current state
        {
            SerializableVector3 position = (SerializableVector3)state; // assigning the value of current state to position
            navMeshAgent.enabled = false; // in beginning navmesh is stopped
            transform.position = position.ToVector();
            navMeshAgent.enabled = true; // after getting the transform.position info navmesh start to working 
            GetComponent<ActionScheduler>().CancelCurrentAction(); // for restoring current state current action stopped
        }
    }
}