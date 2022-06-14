// CombatTarget.cs file stands for arranging target for combat in JaimGame

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Attributes;
using JAIM.Control;

namespace JAIM.Combat // this namespace holds attributes about combat
{
    [RequireComponent(typeof(Health))] // RequireComponent adds required components dependencies in unity, in here we use this for adding health component
    public class CombatTarget : MonoBehaviour, IRaycastable // Monobehaviour is the base class for all created component in unity
    {
        public CursorType GetCursorType() // returns cursor for combat
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
           if(!callingController.GetComponent<Fighter>().AbleToAttack(gameObject)) // if selected game object is not attackable return false
           {
               return false;
           }

           if(Input.GetMouseButton(0)) 
           {
               callingController.GetComponent<Fighter>().Attack(gameObject); // calls the controller that arranges attacking the specifyed game object(enemy)
           }
          
            return true;
        }
    }
} 