// MainMenuUI.cs file arranges the Main Menu's UI 

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.SceneManagement;
using JAIM.Utils;
using TMPro;


namespace JAIM.UI // this namespace holds attributes about UI 

{

    // The Monobehaviour class is the base class from which the script file of each Unity component is derived. We have to use it if we want to create a component 
    public class MainMenuUI : MonoBehaviour
    {

        LazyValue<SavingWrapper> savingWrapper;  // LazyValue only works when it is necessary, in here we are indicating that we will save the wrapper's state when necessary
        [SerializeField] TMP_InputField newGameNameField;

        private void Awake() { // awake works only once, when it is called 
            
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper); // saving wrapper's state by using a lazy value

        }

        private SavingWrapper GetSavingWrapper() // returns the finded gameobject for saving process
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame() // stands for continue to game after saving the wrapper
        {
            savingWrapper.value.ContinueGame();
        }

        public void NewGame() // works when we start a new game
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }

        public void QuitGame(){ // arranges the quit game situation for MainMenu

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying=false; // checking if UI editor is not working
#else
            Application.Quit(); // if UI editor is working makes it stopped
#endif
        }
    }
}
