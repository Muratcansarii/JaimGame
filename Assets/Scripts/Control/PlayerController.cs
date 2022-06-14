// PlayerControllerh.cs file stands for controlling the player in JaimGame

// Adding namespaces that we keep in safe 
using JAIM.Movement;
using JAIM.Combat;
using UnityEngine;
using JAIM.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace JAIM.Control // this namespace holds attributes about control
{

    public class PlayerController : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        Health health;

        [System.Serializable]
        struct CursorMapping // creating a struct for Cursor
        {
        public  CursorType type; // initializing cursortype
        public Texture2D texture; //initializing texture
        public Vector2 hotspot; // initializing hotspot
        }
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField]CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f; //initializing max value of NavmeshprojectionDistance
        [SerializeField] float raycastRadius = 1f; // initializing raycastradius


        private void Awake(){ // this works once only it called and assign the health value
            health = GetComponent<Health>();
        }

    private void Update() // this works once in every frame and interacts with ui tools of player controller file
        {
            if (InteractWithUI()) return; 
            if (health.IsDead()) // checks the isdead state for player
            {
                SetCursor(CursorType.None); // sets a cursortype then returns
                return;
            }
            if (InteractWithComponent()) return; 
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent() // this block of code stands for interacting with components in the game
        {   
            RaycastHit[] hits = RaycastAllSorted(); // defining a hits variable 
            foreach (RaycastHit hit in hits) // traversing between raycasthit to hit
            {
           IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
           foreach (IRaycastable raycastable in raycastables) // traversing between raycastable values
           {
               if(raycastable.HandleRaycast(this)) 
               {   SetCursor(raycastable.GetCursorType());
                   return true;
               }
           }
            }
            return false;
        }

           RaycastHit[] RaycastAllSorted()  // stands for sorting all the raycast
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++) 
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI() // this block of code interacts with ui tools 
        {
            if (EventSystem.current.IsPointerOverGameObject()){ 
                SetCursor(CursorType.UI);
                return true;
            }
                return false;
        }

      

  
        private bool InteractWithMovement() // this block of code stands for interacting movement feature
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target); 
            if (hasHit)
            {   
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if(Input.GetMouseButton(0))
                {
                 GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                 SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 target) // this block of code stands for arranging appearance of controller behaviours
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;               
            return true;
        }

      private void SetCursor(CursorType type) // this block of code stands for ui tools of cursor
        {
            CursorMapping mapping = GetCursorMapping(type); 
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private  CursorMapping GetCursorMapping(CursorType type){ // this block of code stands for arranging cursor mapping
            foreach (CursorMapping mapping in cursorMappings) // traversing in cursormaps
            {
                if(mapping.type == type){ // checks if mapping type is right, if yes then return mapping
                    return mapping;
                }
            }
            return cursorMappings[4];

        }
        private static Ray GetMouseRay() // arranges the view of camera for player using mouse
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition); // arranging camera's position accorgint to mouse's position
        }
    }
}