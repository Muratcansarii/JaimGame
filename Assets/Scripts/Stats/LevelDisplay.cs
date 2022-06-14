// This file is using displaying level to the screen

// Adding namespaces that we keep in safe 
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JAIM.Stats // this namespace holds attributes about Stats
{
    public class LevelDisplay : MonoBehaviour{ // Monobehaviour is the base class for all created component in unity
        BaseStats baseStats; // initializating the basestats
        private void Awake() { // this works just once when it is called
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>(); // specifying the basestats
        }

        private void Update() { // update works once in every frame in the game
            GetComponent<Text>().text = String.Format("{0:0}" , baseStats.GetLevel()); // defining the text about level
        }
    }
}