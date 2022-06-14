// WeaponPickup.cs file stands for creating pickup(gameobject that spawns in the map and we can use) for weapons in JaimGame

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using System.Collections.Generic;
using JAIM.Attributes;
using JAIM.Control;
using UnityEngine;

namespace JAIM.Combat // this namespace holds attributes about combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable // Monobehaviour is the base class for all created component in unity
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] WeaponConfig weapon = null; 
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float timeToRespawn = 5; // indicating Respawn time of gameobject, this is the time it takes for an object to reappear on the map from the time we receive it.

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject) // defining pickup
        {
            if (weapon != null) // checks if the weapon is null, if not null then indicates that player has an equipped weapon
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);

            }
            if (healthToRestore > 0) 
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(timeToRespawn)); // starting the coroutine that works for re-spawning time of the game object
        }

        private IEnumerator HideForSeconds(float seconds) // IEnumerable must be implemented for a class to gain iterator property.
        {
            ShowPickup(false); // dont show the pickup until yield return gives a second value for right spawning time
            yield return new WaitForSeconds(seconds); // yield return used when returning an element to the foreach loop that calls the Iterator
            ShowPickup(true); // after getting the alert of its time to spawn this game object, show the pick up

        }

     

        private void ShowPickup(bool shouldShow) // this code blocks stands for showing the pickup in the map
        {
            GetComponent<Collider>().enabled = shouldShow; // enabling the collider in unity scene
            foreach (Transform child in transform) { 
                {
                    child.gameObject.SetActive(shouldShow); // getting activeted the game object's appearing feature
                }
            }
        }

        public bool HandleRaycast(PlayerController callingController) // this block of code arranges to handle of raycast for weapon pickups
        {
            if (Input.GetMouseButtonDown(0)) // if we click on selected pick up, then take the pick up
            {
                Pickup(callingController.gameObject);
            }
                return true;
        }

        public CursorType GetCursorType() 
        {
           return CursorType.Pickup;
        }
    }
}