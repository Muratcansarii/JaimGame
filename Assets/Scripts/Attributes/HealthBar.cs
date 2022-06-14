// HealtBar.cs file stands for Creating a Bar for Health attribute in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This namespace holds Attributes
namespace JAIM.Attributes
{

    public class HealthBar : MonoBehaviour // Creating HealthBar class
    {
        
        
        [SerializeField] Health healthComponent = null; // Health created as healthcomponent in unity engine
        [SerializeField] RectTransform foreground = null; // RectTransform which specifies that this is a ui element created in unity engine
        [SerializeField] Canvas rootCanvas = null;  // Canvas which specifies that this ui tool will using in scene created in unity engine

        
        void Update() // update method works in every frame
        {
            // we use Mathf and specifying if necessary requirements are not realized, HealthBar ui tool wont displayed in the game with rootcanvas.enabled = false statement
            if (Mathf.Approximately(healthComponent.GetFraction(), 0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            {
                rootCanvas.enabled = false; 
                return; 
            }

            rootCanvas.enabled = true; // returns the closest canvas to root, when it enabled specified ui tool displayed specified place.
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1); // foreground.localscale changes health bar's situation, so when we get a damage and our health decrease
            // 10 to 6, heath bar's lenght decreate 10 unit to 6 unit.

        }
    }
}
