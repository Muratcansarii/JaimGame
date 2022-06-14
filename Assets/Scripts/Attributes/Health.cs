// Healt.cs file stands for arranging Health feature of JaimGame

// Adding namespaces that we keep in safe 
using System;
using JAIM.Utils;
using JAIM.Core;
using JAIM.Saving;
using JAIM.Stats;
using UnityEngine;
using UnityEngine.Events;

// This namespace holds attributes
namespace JAIM.Attributes
{
    public class Health : MonoBehaviour, ISaveable 
    // The Monobehaviour class is the base class from which the script file of each Unity component is derived.
    // If you want to create any component, it must be derived directly or indirectly from the Monobehaviour class.
    //Isaveable specifies that this class can saveable and reusable.
    {
        // Variables that created with SerializedField will also created in our Unity Game Engine.
        [SerializeField] float regenPercentage = 80; // Assigning a value to Percentage
        [SerializeField] TakeDamageEvent takeDamage; // Defining takeDamage variable
        public UnityEvent onDie; // Defining an Unity Event that triggered on die.

        [System.Serializable] // Indicating that this is a seralizable class which can be saved from unity engine for storing data.
        public class TakeDamageEvent : UnityEvent<float> // Defining TakaDamageEvent class.
        {
        }

        LazyValue<float> healthPoints; // Defining healthPoints as a LazyValue so it will work only when needed.

        bool wasDeadEndFrame = false; // Defining a bool variable wasDeadEndFrame to "false", 

        private void Awake() // Awake only works once when the game starts.
        {
            healthPoints = new LazyValue<float>(GetStartingHealth); // Defining a healthpoint variable that works only when needed and will get the startinghealth
        }

        private float GetStartingHealth() // this method gets the starting health
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health); // returning the Health component's first value
        }

        private void Start() // this method works only once when the game start like Awake(), Awake works once when only needed but Start works once in the beginning without any needed call.
        {
            healthPoints.ForceInit(); // Initializing the healthpoints 
        }



        private void OnEnable() // When Enabled, Increase the Health in level ups via adding RegenHealth to Health
        {
            GetComponent<BaseStats>().onLevelUp += RegenHealth;
        }

        private void OnDisable() // When Disabled, Decrease the Health in level ups via subtructing RegenHealth from Health
        {
            GetComponent<BaseStats>().onLevelUp -= RegenHealth;
        }

        public bool IsDead() // When Health = 0 our player will die, for this we use IsDead() method.
        {
            return healthPoints.value<= 0;
        }

        public void TakeDamage(GameObject instigator, float damage) // this block of code stands for arranging health when character taking damage
        {
           

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); // subtraction damage value from health's value
            if (IsDead()) // checking chracter dead or still alive
            {
               onDie.Invoke();
               //Die();
                AwardExperience(instigator);
             } 
            else
            {
                 takeDamage.Invoke(damage);
            }
            UpdateState(); // updates the current state
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState(); // updates the current state
        }

        public float GetHealthPoints() // returns the health's value
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints() // returns the maximum of health
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercent() // returns the percenteage of current health 
        {
            return 100 * GetFraction();
        }

        
        public float GetFraction() // returns the current health's value over total health's value like 60/100
        {

            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);

        }

        private void UpdateState() // updates unity engine's state about health 
        {

            Animator animator = GetComponent<Animator>();
            if(!wasDeadEndFrame && IsDead())
            {

                animator.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();

            }

            if(wasDeadEndFrame && !IsDead()){

                animator.Rebind(); 
                
            }
            

            wasDeadEndFrame = IsDead(); // calling isdead function 

        }
        
    

        private void AwardExperience(GameObject instigator) // This block of code responsible for giving an health award for level up.
        {
            Experience experience = instigator.GetComponent<Experience>(); 
            if (experience == null) return; // indicating that if there is no experience there is no award

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward)); // 
        }

        private void RegenHealth() // regenerating the health. This block of code calculates the health's current new situation.
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState() // capturing the health's current state
        {
            return healthPoints.value; // returning health's current value
        }

        public void RestoreState(object state) // Updating the healt's current situation
        {
            healthPoints.value = (float)state; // assigning healt's current value to state variable

            UpdateState(); // calling updatestate function
            
        }
    }
}