// AIController.cs file stands for arranging the AI in JaimGame

// Adding namespaces that we keep in safe 
using JAIM.Combat;
using UnityEngine;
using JAIM.Core;
using JAIM.Movement;
using JAIM.Attributes;
using JAIM.Utils;
using System;
using UnityEngine.AI;

namespace JAIM.Control // this namespace holds attributes about control
{

    public class AIController: MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] float chaseDistance = 5f; // defining the chase distance, this is the range where the enemy attacks when entered
        [SerializeField] float suspicionTime = 3f; // defining suspiciont time, the time it takes for the enemy to stop spying the player when player quits enemy's range
        [SerializeField] float agroCooldownTime = 5f; // defininf cooldown time, this is the time that takes for the enemy to stop chasing when out of range of the enemy 
        [SerializeField] PatrolPath patrolPath; // defining patrolpath, this is the path that some of the enemy's patrolling along the path
        [SerializeField] float waypointTolerance = 1f; // defining waypoint tolerance, this is the amount of tolerance to enter the patrol range of the patrolling enemy
        [SerializeField] float waypointDwellTime = 3f; 
        [Range(0,1)] 
        [SerializeField] float patrolSpeedFraction = 0.2f; // defining the speed of enemy that doing patrolling
        [SerializeField] float shoutDistance = 5f; // defining shout distance, this is a distance that necessary for hitting a target

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        LazyValue<Vector3> guardPosition; // lazy values only called when they are called/necessery, in here we are assigning guard's position to a lazy value
        float timeSinceLastSawPlayer = Mathf.Infinity; // defining the time when enemy saw lastly the player
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; // defining the time of arriving a waypoint in patrolpath
        float timeSinceAggrevated = Mathf.Infinity; 
        int currentWaypointIndex = 0; // defining current waypoint's index value

        private void Awake() // awake works only once, when called
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition() // specifying the guard's current position 
        {
            return transform.position;
        }

        public void Reset() // This function is only called in editor mode. Reset is most commonly used to give good default values in the inspector.
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>(); // defining navmeshagent
            navMeshAgent.Warp(guardPosition.value); // defining navmeshagent's warp feature
            timeSinceLastSawPlayer = Mathf.Infinity; // this is the time when enemy saw our player lastly
            timeSinceArrivedAtWaypoint = Mathf.Infinity; // this is the time when enemy arrives way point
            timeSinceAggrevated = Mathf.Infinity; // this is the time that enemy aggrevated
            currentWaypointIndex = 0;

        }

        private void Start() // start works only once in the game, in here we are arranging the guard's current position
        {
            guardPosition.ForceInit(); // arranges guard position
        }


        private void Update() // update works once in every frame
        {
            if (health.IsDead()) return; // checks dead or not situation, if it's dead returns
            if (IsAggrevated() && fighter.AbleToAttack(player)) // checks if its aggrevated and player can attack then calles attack behaviour
            {
                AttackBehaviour();  
            }

            else if (timeSinceLastSawPlayer < suspicionTime) // else checks lat saw time is smaller than or not to suspicion time, if its smaller than calles suspicion behaviour
            {
                SuspicionBehaviour();
            }
            else // else calles patrol behaviour because suspicion ended and enemy should start the patrolling again
            {
                PatrolBehaviour();
            }

            UpdateTimers(); // update timer in every frame
        }

        public void Aggrevate() // aggrevate method
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers() // arranging time updates of last saw player, arrived time of waypoint and aggrevated situations
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        void AttackBehaviour() // arranging attack behaviour
            {
                timeSinceLastSawPlayer = 0; // assigning last saw time to zero
                fighter.Attack(player); 
                AggrevateNearbyEnemies();
            }

        private void AggrevateNearbyEnemies() // activating hits when player on the near of enemies.
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null ) continue;

                ai.Aggrevate(); 
            }
        }

        private void SuspicionBehaviour() // this is the behaviour of When the player leaves the enemy's area, the enemies are watching the player for a specidied time.
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour() // this is the behaviour of enemy patrolling, enemies moves around a specified patrolpath.
        {
            Vector3 nextPosition = guardPosition.value; // assigning value of guard's current position
            if(patrolPath != null){ // checks if patrolling path is null or not

                if(AtWaypoint()){ // if its at way point, then makes 0 to time that indicate arriving way point then calling cyclewaypoint()

                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();

                }
                nextPosition= GetCurremtWaypoint(); // assign current way point situation
            }
            if(timeSinceArrivedAtWaypoint > waypointDwellTime ){ // checks time that indicate arriving way point is bigger way point's dwell time

                mover.StartMoveAction(nextPosition, patrolSpeedFraction); // if its bigger starts a moving action to next position 

            }
                
        }
        

        private bool AtWaypoint()
        {   
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurremtWaypoint()); // calculating waypoint distance
            return distanceToWaypoint < waypointTolerance;  // returns waypoint situation
        }

        private void CycleWaypoint()  // this block of code stands for arranging cyclewaypoint behaviour
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurremtWaypoint() // arranges returning current way point
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool IsAggrevated(){ // this bool value stands for arranging aggrevated situation

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); // value of distance between enemy and player
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime; 
        }


        // This block of code called directly by unity engine. It handles speciying appearance of gizmos
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue; // makes gizmos color blue
            Gizmos.DrawWireSphere(transform.position, chaseDistance); // makes gizmos shape wire
        }


    }



}