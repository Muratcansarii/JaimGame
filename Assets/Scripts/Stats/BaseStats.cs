// BaseStats.cs file responsible of stats in Jaim Game

// Adding namespaces that we keep in safe 
using System;
using System.Collections;
using System.Collections.Generic;
using JAIM.Utils;
using UnityEngine;

namespace JAIM.Stats // this namespace holds attributes about Stats
{
    public class BaseStats : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        [Range(1, 99)] // defining the range value between 1 and 98
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] int startingLevel = 1; // defining starting level = 1
        [SerializeField] CharacterClass characterClass; // defining characterclass 
        [SerializeField] Progression progression = null; // defining progression = null at the beginning
        [SerializeField] GameObject levelUpParticleEffect = null; // defining level up effect to null
        [SerializeField] bool shouldUseModifiers = false; // defining using modifiers to false at the beginning

        public event Action onLevelUp; // onlevelup defining as a public event action

        LazyValue<int> currentLevel; // lazy values works only it is necessary

        Experience experience;

        private void Awake() // this code block works only once when it is called, it defining the experience and current level behaviours
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() // this code block works once when the game starts, it inits the current level
        {
            currentLevel.ForceInit();
        }

        private void OnEnable() // enable case 
        {
            if (experience != null) // checks the experience is null or not, if not then it updates the current experience
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() // disable case
        {
            if (experience != null) // checks the experience is null or not, if not then it updates the current experience
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() // this code block stands for updating the level
        {
            int newLevel = CalculateLevel(); // defining a newlevel
            if (newLevel > currentLevel.value) // checks if new level is bigger than current level or not, if its bigger then make new level to current level
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect() // stands for level up ui effect
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) // stands for getstat behaviour
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat) // stands for get base stat behaviour
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() // returns the current level's value
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat) 
        {
            if (!shouldUseModifiers) return 0; 

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) // traversing
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier; // adding modifier value to the total value
                }
            }
            return total; // returning the total value
        }

        private float GetPercentageModifier(Stat stat) // this block of code arranges to get the modifier's percentage
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) // traversing
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel() // this block of code responsible for calculating the level
        {
            Experience experience = GetComponent<Experience>(); // defining experince
            if (experience == null) return startingLevel; // if experience is null then return starting level

            float currentXP = experience.GetPoints(); // assigning gained experience to current experience
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP) // checks experience that needed for level up is greater than current xp, then returns level
                {
                    return level;
                }
            }

            return penultimateLevel + 1; // returns the level info of one before the last level
        }
    }
}