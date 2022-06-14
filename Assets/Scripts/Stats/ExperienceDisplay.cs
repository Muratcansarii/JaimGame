// ExperienceDisplay.cs file arranges displaying the experience feature of Jaim Game

// Adding namespaces that we keep in safe 
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JAIM.Stats // this namespace holds attributes about Stats
{ 

    public class ExperienceDisplay : MonoBehaviour{ // Monobehaviour is the base class for all created component in unity


        Experience experience; // defining an experience value

        private void Awake() { // this method work only once in the game, when it called
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>(); // arranges the experience of player
        }


        private void Update() { // this method works once in every frame
            
            GetComponent<Text>().text = String.Format("{0:0}" , experience.GetPoints()); // shows the current experience level of player
        }

    }


}