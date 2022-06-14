// SaveLoadUI.cs file arranges the Main Menu UI of Save-Load Situation

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace JAIM.UI{ // this namespace holds attributes about UI 
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class SaveLoadUI : MonoBehaviour
    {
        //SerializedField allow us to define variables in our Unity Game Engine.So when we create a variable with SerializedField it also created in Unity Engine.
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject buttonPrefab;

           private void OnEnable()  // enabling the save-load feature 
           {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>(); // stands for initializing the savingwrapper feature
            if (savingWrapper == null) return; // checks savingwrapper is null or not, if null then returns
                foreach (Transform child in contentRoot)
                {
                   Destroy(child.gameObject); 
                }
               
                foreach(string save in savingWrapper.ListSaves()) // Lists the saved games
                {
                GameObject buttonInstance = Instantiate(buttonPrefab,contentRoot); // creates a button 
                TMP_Text textComp = buttonInstance.GetComponentInChildren<TMP_Text>();
                textComp.text=save;

                Button button = buttonInstance.GetComponentInChildren<Button>();

                button.onClick.AddListener(() => // adding a button to save-load feature, for doing these we are adding a listener to a button by using onclick and addlistener methods
                {
                    savingWrapper.LoadGame(save); // loads the last saved game
                });
                }
           }
    }


}


