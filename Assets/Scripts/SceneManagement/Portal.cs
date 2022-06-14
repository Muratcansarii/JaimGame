// Portal.cs file stands for arranging portal's feature in JaimGame

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using JAIM.Saving;
using JAIM.Control;

namespace JAIM.SceneManagement // this namespace holds attributes about scene management
{
    public class Portal : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        enum DestinationIdentifier // defining enums for destinations
        {
            A, B, C, D, E
        }
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] int sceneToLoad = -1; // scnetoload indicates the scene that will be loaded, in the beginning its -1
        [SerializeField] Transform spawnPoint; // defining the spawnpoint for portal
        [SerializeField] DestinationIdentifier destination; // defining destination
        [SerializeField] float fadeOutTime = 1f; // defining fadeout/in/wait times
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) // this block of code stands for checking triggering situation for portalling
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() // this code block stands for handling the transition.
        {
            if (sceneToLoad < 0) // checks is there a scene to load, if there is no then displays a error message and returns.
            {
                Debug.LogError("Scene to load not set."); // displaying this message to log screen 
                yield break; 
            }

            DontDestroyOnLoad(gameObject); 

            Fader fader = FindObjectOfType<Fader>();

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>(); // saves the current situation
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); // initializing player controller
            playerController.enabled = false; // for this state player controller is disabled


            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save(); // saving wrapper

            yield return SceneManager.LoadSceneAsync(sceneToLoad); 
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            wrapper.Load();

            Portal otherPortal = GetOtherPortal(); 
            UpdatePlayer(otherPortal); // update the player's situation

            wrapper.Save(); // saves current progress

            yield return new WaitForSeconds(fadeWaitTime); // waits until fadewaittime end
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true; // enabling the new player controller
            Destroy(gameObject); // destroys the gameobject
        }

        private void UpdatePlayer(Portal otherPortal) // updates the player's situation 
        {
            GameObject player = GameObject.FindWithTag("Player");  // selects the player game object
            player.GetComponent<NavMeshAgent>().enabled = false; 
            player.transform.position = otherPortal.spawnPoint.position; // arranges the transform's position for spawn point
            player.transform.rotation = otherPortal.spawnPoint.rotation; // arranges the transform's position for spawn rotation
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal() // stands for other portals
        {
            foreach (Portal portal in FindObjectsOfType<Portal>()) // traversing portals until finging specifyed one
            {
                if (portal == this) continue; // if founded portal is specifyed one then continue
                if (portal.destination != destination) continue; // if portal's destination is not destination then continue

                return portal; // return the portal
            }

            return null;
        }
    }
}