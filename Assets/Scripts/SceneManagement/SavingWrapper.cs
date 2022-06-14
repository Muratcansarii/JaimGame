// SavingWrapper.cs file arranges saving the wrapper in Jaim Game

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using System.Collections.Generic;
using JAIM.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAIM.SceneManagement // this namespace holds attributes about scene management
{
    public class SavingWrapper : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        
        private const string currentSaveKey = "currentSaveName"; // appoint a key for saving current game
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] float fadeInTime = 0.2f;  // defining fadeintime
        [SerializeField] float fadeOutTime = 0.2f; // defining fadeouttime
        [SerializeField] int   firstLevelBuildIndex = 1; // defining firstlevelbuildindex
        [SerializeField] int   menuLevelBuildIndex = 0; // defining menulevelbuildindex
        public void ContinueGame() // continue the game after saving wrapper state
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene()); // coroutine start to work for loading the last scene
        }

        public void NewGame(string saveFile) // creating a newgame
        {
            if (string.IsNullOrEmpty(saveFile)) return; // checks the string for new game's name, then return
            SetCurrentSave(saveFile); // else saves current state
            StartCoroutine(LoadFirstScene()); // start coroutine and load first scene of the new game
        }

        public void LoadGame(string saveFile) // load an existing game
        {
            SetCurrentSave(saveFile); // selects the saved existing game
            ContinueGame(); // continue to game
        }

        public void LoadMenu() // loads the ui menu for Scene Management
        {
            StartCoroutine(LoadMenuScene());
        }

        private void SetCurrentSave(string saveFile) // sets the current state
        {
            PlayerPrefs.SetString(currentSaveKey,saveFile); 
        }

        private string GetCurrentSave() // gets the saved information about current state
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadMenuScene() // loads the UI Menu in scene
        {
            Fader fader = FindObjectOfType<Fader>(); // arranging the fader
            yield return fader.FadeOut(fadeOutTime); 
            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadFirstScene() // stands for loading the first scene
        {
            Fader fader = FindObjectOfType<Fader>(); // arranging the fader
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }


        private IEnumerator LoadLastScene() // stands for loading the last scene
        {
            Fader fader = FindObjectOfType<Fader>(); // arranging the fader
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() // update method work once in every frame, it checks the state of keys like if the user press a key or not in every frame
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load() // for loading a scene
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Save() // for saving a scene
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Delete() // for deleting a saved file
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves() // this code stands for listing the saved games
        {

            return GetComponent<SavingSystem>().ListSaves();
        }

    }
}