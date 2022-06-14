// SavingSystem.cs file stands for saving the system in JaimGame

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAIM.Saving // this namespace holds attributes about Saving
{
    public class SavingSystem : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        public IEnumerator LoadLastScene(string saveFile) // this block of code stands for loading the last scene
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        public void Save(string saveFile) // this block of code stands for saving the selected file
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile) // loads the previously saved file
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile) // deletes the previously saved file
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public IEnumerable<String> ListSaves() // listing previously saved files
        {
           foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
           {
               if (Path.GetExtension(path) == ".sav")
               {
                   yield return Path.GetFileNameWithoutExtension(path);
               }
           }
        }

        public bool SaveFileExists(string saveFile) 
        {
            string path = GetPathFromSaveFile(saveFile);
            return File.Exists(path);

        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFile, object state) // this block of code stands for saving file, it takes savefile and state parameters
        {
            string path = GetPathFromSaveFile(saveFile); // assigning path value
            print("Saving to " + path); // printing the saving path info
            using (FileStream stream = File.Open(path, FileMode.Create)) // for opening the selected file
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state) // captures the state of game object
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) //traversing between saveable entities
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex; 
        }

        private void RestoreState(Dictionary<string, object> state) // update the state of restored game object
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) // traversing between saveable entities
            {
                string id = saveable.GetUniqueIdentifier(); // assigning value to saveable entity
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile) // gets the path info from saved file
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}