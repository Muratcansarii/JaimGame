// PatrolPath.cs file stands for defining a patrolling path for enemies in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.Control // this namespace holds attributes about control
{

    public class PatrolPath : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {

        const float waypointGizmosRadius = 0.3f; // defining a gizmos radius for waypoints.
    
        private void OnDrawGizmos() { // specifying the appearance of patrolling path
            
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }

       }

        public int GetNextIndex(int i) // calculates the next position of waypoint
        {
            if(i + 1 == transform.childCount){

                return 0; 

            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i) // returns the position of current waypoint
        {
            return transform.GetChild(i).position;
        }

    }

}