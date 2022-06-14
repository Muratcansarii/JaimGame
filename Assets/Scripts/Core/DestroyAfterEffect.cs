// DestroyAfterEffect.cs file stands for handling effects of starts after destroying an gameobject in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.Core // this namespace holds attributes about Core
{
    public class DestroyAfterEffect : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        [SerializeField] GameObject targetToDestroy = null; // SerializedField allow us to make a copy of our created variables in unity engine
        // in here we are specifying that destroy target is null in the beginning

        void Update() // update works once in every frame
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) // checks if the gameobject is not alive or alive, if not alive then starts to destroy gameobject
            {
                if (targetToDestroy != null) // checks destroy target is null or not
                {
                    Destroy(targetToDestroy); 
                }
                else
                {
                    Destroy(gameObject); // destroys the gameobject
                }
            }
        }
    }
}