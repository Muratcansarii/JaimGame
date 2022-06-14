// This file stands for object spawning

using System;
using UnityEngine;

namespace JAIM.Core // this namespace holds attributes about Core
{


public class PersistentObjectSpawner : MonoBehaviour  // Monobehaviour is the base class for all created component in unity
    {
        [SerializeField] GameObject persistentObjectPrefab; // SerializedField allow us to make a copy of our created variables in unity engine

        static bool hasSpawned = false; // in the beginning hasSpawned behaviour is false
        private void Awake(){ // this works only once when it is called
            if(hasSpawned) return; // returns the spawned gameobject
            SpawnPersistentObjects();
            hasSpawned = true; // after returning the gameobject hasSpawned behaviour becomes true
        }

        private void SpawnPersistentObjects() // arranges spawning the persistent game objects
        {
           GameObject persistentObject = Instantiate(persistentObjectPrefab);
           DontDestroyOnLoad(persistentObject);
        }
    }
}