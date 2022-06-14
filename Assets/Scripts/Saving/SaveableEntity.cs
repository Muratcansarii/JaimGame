// SaveableEntity.cs file stands for arranging saveable objects in JaimGame

// Adding namespaces that we keep in safe 
using System;
using System.Collections.Generic;
using JAIM.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace JAIM.Saving // this namespace holds attributes about Saving
{
    [ExecuteAlways] // defining that this will executing always
    public class SaveableEntity : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        [SerializeField] string uniqueIdentifier = ""; // SerializedField allow us to make a copy of our created variables in unity engine
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier() // returns unique identifier
        {
            return uniqueIdentifier;
        }

        public object CaptureState() // this block of code stands for capturing the state
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>()) // traversing between saveable values
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state) // this block of code stands for restoring the current state
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>()) // traversing between saveable values
            {
                string typeString = saveable.GetType().ToString(); // assigning stypestring value
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update() { // update function, it checks the current situation once in every frame
            if (Application.IsPlaying(gameObject)) return; 
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue)) // checks if string value empty or not
            {
                property.stringValue = System.Guid.NewGuid().ToString(); // makes string of new guid
                serializedObject.ApplyModifiedProperties(); // appliying last changes
            }

            globalLookup[property.stringValue] = this; // appointing the selected value to strtingvalue
        }
#endif

        private bool IsUnique(string candidate) // this bool value check if candidate unique or not
        {
            if (!globalLookup.ContainsKey(candidate)) return true; 

            if (globalLookup[candidate] == this) return true; // checks if candidate is selected object or not

            if (globalLookup[candidate] == null) // checks if canditate is null or not
            {
                globalLookup.Remove(candidate); // removes the candidate
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate) // checks the unique identifier is equal to candidate or not
            {
                globalLookup.Remove(candidate); // removes the candidate
                return true;
            }

            return false; // now the value is not unique so it returns false
        }
    }
}