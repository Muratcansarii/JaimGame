// HealtDisplay.cs file stands for displaying Health Status in JaimGame

// Adding namespaces that we keep in safe 
using System;
using UnityEngine;
using UnityEngine.UI;

// This namespace holds Attributes
namespace JAIM.Attributes 
{
    public class HealthDisplay : MonoBehaviour // Creating HealthDisplay class
    {
        Health health; // Health variable defining

        private void Awake() // Awake Method is the first method when the game is starting, We want to show health status since the game start.
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>(); // Getting player gameobject's health component and assingning it to health variable.
        }

        private void Update() // Update Method works once in every frame. We want to display health status in each frame.
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); // Displays Current Healt over Max Healt line 80/100.
        }
    }
}