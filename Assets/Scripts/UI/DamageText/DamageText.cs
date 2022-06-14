// DamageText.cs file arranges to displaying the damage as a text form in screen

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


namespace JAIM.UI.DamageText // this namespace holds attributes about UI for displaying the damages in the screen
{
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class DamageText : MonoBehaviour 
    {
         [SerializeField] Text damageText = null; // SerializedField allow us to make a copy of our created variables in unity engine
        public void DestroyText() // for removing the text in the screen
        {
            Destroy(gameObject); // as text objects also a gameobject we can use this expression
        }
           public void SetValue(float amount) // defining the damage value that will be displayed in the game 
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
} 