// EnemyHealthDisplay.cs file stands for displaying enemy's health in JaimGame

// Adding namespaces that we keep in safe 
using System;
using JAIM.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace JAIM.Combat // this namespace holds attributes about combat
{
    public class EnemyHealthDisplay : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        Fighter fighter;

        private void Awake() // awake works only once in game when it called
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); // finds the player as fighter game object
        }

        private void Update() // update works once in every frame in unity
        {
            if (fighter.GetTarget() == null) // if there is no target do nothing
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();  
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); // displays the current healt over max health like 6/10
        }
    }
}