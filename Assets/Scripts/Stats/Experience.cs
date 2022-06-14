// Experience.cs file arranges the experience feature of Jaim Game

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Saving;
using System;

namespace JAIM.Stats // this namespace holds attributes about Stats
{
    public class Experience : MonoBehaviour, ISaveable  // Monobehaviour is the base class for all created component in unity
    {
        [SerializeField] float experiencePoints = 0; // SerializedField allow us to make a copy of our created variables in unity engine

        public event Action onExperienceGained; // defining a public event about doing an action when player gains experience


        public void GainExperience(float experience) // when player gets experience this block of code adds it the experience points
        {
            experiencePoints += experience; 
            onExperienceGained();
        }


        public float GetPoints() // returns the experience points
        {
            return experiencePoints;
        }


        public object CaptureState() // capture state for experience behaviour
        {
            return experiencePoints;
        }


        public void RestoreState(object state) // restore state for experience behaviour
        {
            experiencePoints = (float)state;
        }
    }
}