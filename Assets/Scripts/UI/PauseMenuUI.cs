// PausedMenuUI.cs file arranges the Main Menu UI of Pause Situation

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Control;
using JAIM.SceneManagement;

namespace JAIM.UI{ // this namespace holds attributes about UI 

    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerController playercontroller; 
        private void Awake() { // awake works only once, when it is called 
            playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
        }

        private void OnEnable() { // enable case of Pause
            if(playercontroller == null) return; // checks playercontroller and if there is nothing it returns
            Time.timeScale = 0;  // makes time stopped, because game has been paused
            playercontroller.enabled = false; // makes playar controller stopped because game has been paused
        }

        private void OnDisable() { // disable case of pause
            Time.timeScale = 1; // makes time works again, because we quit pause state and now our game continue to work
            playercontroller.enabled = true; // makes player controller work again, because we quit the pause state and now our game continue to work
        }

        public void Save() // save button of menu, we can save our game by using this buttton
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save(); // saves the current game
        }

        public void SaveAndQuit() // save and quit button, we can save and then quit of our game by using this code block
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>(); 
            savingWrapper.Save(); // saves the current game
            savingWrapper.LoadMenu(); // loads the menu
        }

    }




}




