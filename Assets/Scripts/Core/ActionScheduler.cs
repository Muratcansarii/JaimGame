// ActionScheduler.cs file stands for scheduling the actions in JaimGame

using UnityEngine;

namespace JAIM.Core // this namespace holds attributes about Core
{
    public class ActionScheduler : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        IAction currentAction; // defining action 

        public void StartAction(IAction action) // starting an action
        {
            if(currentAction == action) return; // checks current action equals or not to action behavior, if yes then return
            if(currentAction != null) // checks current action is null or not, if not then stops the current action
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction(){ // canceling the current action

            StartAction(null); // giving the startaction to null value

        }
    }
}