// UISwitcher.cs file stands for switching UI 

using UnityEngine;

namespace JAIM.UI // this namespace holds attributes about UI 
{
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class UISwitcher : MonoBehaviour
    {

        //SerializedField allow us to define variables in our Unity Game Engine.So when we create a variable with SerializedField it also created in Unity Engine.
        [SerializeField] GameObject entryPoint;

        private void Start() { // start only works once when called, in here its defining that when game in the start phase switch to entrypoint state.
            SwitchTo(entryPoint);
        }
            public void SwitchTo(GameObject toDisplay) // this block of codes stands for switching one object to another.
            {
                if(toDisplay.transform.parent != transform) return;
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(child.gameObject == toDisplay);
                }
                
            }
    }
}

