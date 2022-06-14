// CinematicControlRemover.cs file stands for arranging cinematics in JaimGame

// Adding namespaces that we keep in safe 
using UnityEngine;
using UnityEngine.Playables;
using JAIM.Core;
using JAIM.Control;

namespace JAIM.Cinematics // this namespace holds attributes about cinematics
{
    public class CinematicControlRemover : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        GameObject player; // specifying the player gameobject

        private void Awake() // awake only work once when it called
        {
            player = GameObject.FindWithTag("Player"); // defining the player gameobject will used
        }

        private void OnEnable() // takes care of enable state
        {
            GetComponent<PlayableDirector>().played += DisableControl;  
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() // takes care of disable state
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector pd) // controls the disabling
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction(); // disables the current working action
            player.GetComponent<PlayerController>().enabled = false; // makes player controller disable
        }

        void EnableControl(PlayableDirector pd) // controls the enabling, 
        {
            player.GetComponent<PlayerController>().enabled = true; // makes player controller  enable
        }
    }
}