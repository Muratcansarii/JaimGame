// ShowHideUI.cs file stands work showing-hiding of Menu UI 

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.UI  // this namespace holds attributes about UI 
{
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class ShowHideUI : MonoBehaviour
    {
        //SerializedField allow us to define variables in our Unity Game Engine.So when we create a variable with SerializedField it also created in Unity Engine.
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;

        // Start works only once when called, in the first phase of the game it automatically closed
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame, in here we are checking if user want to see UI Menu.
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}