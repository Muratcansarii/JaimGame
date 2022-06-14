// Fader.cs file arranges the fader ui tool's attribute in Jaim Game

// Adding namespaces that we keep in safe 
using System.Collections;
using UnityEngine;

namespace JAIM.SceneManagement // this namespace holds attributes about scene management
{
    public class Fader : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        CanvasGroup canvasGroup; // defining canvas
        Coroutine currentActiveFade = null; // create a coroutine for currentactivefade and make it null in the beginning

        private void Awake() // only work once, when it called.
        {
            canvasGroup = GetComponent<CanvasGroup>();  // initializing the canvas group
        }

        public void FadeOutImmediate()  
        {
            canvasGroup.alpha = 1; 
        }

        public Coroutine FadeOut(float time) // coroutine for fadeout, it takes time parameter
        {
            return Fade(1, time);
           
        }

        public Coroutine FadeIn(float time) // coroutine for fadein, it takes time parameter
        {
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time) // coroutine for Fade, it takes target and time parameters
        {
            if (currentActiveFade != null) // checks if current active fade is not null, then stop current active fade
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time)); // starts a new coroutine and returns it
            return currentActiveFade;

        }

        private IEnumerator FadeRoutine(float target, float time) // creating faderoutine method, it takes target and time parameters
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) // arranges the appeareance of Canvas and UI tools
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
                yield return null;
            }
        }
    }
}