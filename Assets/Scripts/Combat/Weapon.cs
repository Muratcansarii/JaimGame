// Weapon.cs file stands for invoking onhit method in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JAIM.Combat // this namespace holds attributes about combat
{

    public class Weapon : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        [SerializeField] UnityEvent onHit; // SerializedField allow us to make a copy of our created variables in unity engine

        public void OnHit() 
        {
            onHit.Invoke(); // Invokes onhit 
            
        }
    }
}