// CinematicTrigger.cs file stands for checking triggered cinematics in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace JAIM.Cinematics // this namespace holds attributes about cinematics

{
    public class CinematicTrigger : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        bool alreadyTriggered = false; // creating a bool variable that specifying triggered state to false

        private void OnTriggerEnter(Collider other) // Starting the triggering
        {
            if (!alreadyTriggered && other.gameObject.tag == "Player")  //checking if trigger is not already working and current target gameobject is the player
            {
                alreadyTriggered = true; // triggered state becomes true for starting the cinematic
                GetComponent<PlayableDirector>().Play(); // start the cinematic
            }
        }
    }
}