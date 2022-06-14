// Respawner.cs file stands for arranging re-spawn objects in JaimGame

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using Cinemachine;
using JAIM.Attributes;
using JAIM.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace JAIM.Control{ // this namespace holds attributes about control

    public class Respawner : MonoBehaviour  // Monobehaviour is the base class for all created component in unity
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] Transform respawnLocation; // defining a re-spawn location
        [SerializeField] float respawnDelay = 3;   // defining delay for re-spawn
        [SerializeField] float fadeTime = 0.2f; // defining fadetime for re-spawn
        [SerializeField] float healthRegenPercentage = 20; // defining health regeneration percentage for player
        [SerializeField] float enemyHealthRegenPercentage = 20; // defining health regeneration percentage for enemies
        private void Awake() { // this works only once, when it called
            GetComponent<Health>().onDie.AddListener(Respawn); // adding a listener to health
        }
        
        private void Start() { // this works only once in the beginning of the game
            if(GetComponent<Health>().IsDead()) // checks if the gameobject is dead, then calls Respawn() method
            {
                Respawn();
            }

        }

        private void Respawn() // starts a coroutine for re-spawning game objects
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()  // IEnumerable must be implemented for a class to gain iterator property, in here we are adding it for respawnroutine
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>(); // finds the gameobject that will saved
            savingWrapper.Save(); // saves the current situation of the game object
            yield return new WaitForSeconds(respawnDelay); // for delaying re-spawn of the gameobject we are using waitforsecond() method
            Fader fader = FindObjectOfType<Fader>(); // finding the game object with using fader
            yield return fader.FadeOut(fadeTime);  //fader returns a value 
            RespawnPlayer(); // calling re-spawning the player
            ResetEnemies(); // calling re-spawning the enemies
            savingWrapper.Save(); // saves the current situation
            yield return fader.FadeIn(fadeTime); 
        }

        private void ResetEnemies() // this block of code stands for re-spawning the enemies
        {
            foreach (AIController enemyControllers in FindObjectsOfType<AIController>()) // traversing for enemycontrolling
            {
                Health health = enemyControllers.GetComponent<Health>(); // defining a enemy's health
                if(health && !health.IsDead()) // checks if the enemy still alive 
                {
                    enemyControllers.Reset();  // calling reset for enemycontroller
                    health.Heal(health.GetMaxHealthPoints()* enemyHealthRegenPercentage/100); // calculates the new health value
                }
            }
        }

        private void RespawnPlayer() // this block of code stands for re-spawning the player 
        {
            Vector3 positionDelta = respawnLocation.position- transform.position; // arranging the position of the game object
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);  // arranging the re-spawn gameobject location
            Health health = GetComponent<Health>(); 
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100); // calculates the health
            
            ICinemachineCamera activeVirtualCamera  = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera; // checks if camera can find the targeted place
            if(activeVirtualCamera.Follow == transform) // checks the camera follow transform, if yes updates it.
            {
                activeVirtualCamera.OnTargetObjectWarped(transform,positionDelta );
            }
        }
    }



}