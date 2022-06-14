// DamageTextSpawner.cs file arranges to spawning the damage 

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.UI.DamageText // this namespace holds attributes about UI for displaying the damages in the screen
{
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class DamageTextSpawner : MonoBehaviour 
    {
        [SerializeField] DamageText damageTextPrefab = null; // SerializedField allow us to make a copy of our created variables in unity engine


        public void Spawn(float damageAmount) // this block of code takes the amount of damage and spawns it
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
                instance.SetValue(damageAmount); 
        }
    }
}