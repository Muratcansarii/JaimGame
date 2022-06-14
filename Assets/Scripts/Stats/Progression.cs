// Progression.cs file arranges the progression feature of Jaim Game

// Adding namespaces that we keep in safe 
using UnityEngine;
using System.Collections.Generic;
using System;

namespace JAIM.Stats{ // this namespace holds attributes about Stats


    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)] // creating an asset menu named that Progression, Menu Name = Stats/New Progression
    public class Progression : ScriptableObject // defining that progression is a scriptableobject
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] ProgressionCharacterClass[] characterClasses = null; // at the beginning character classes = null

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null; 

        public float GetStat(Stat stat, CharacterClass characterClass, int level) // stands for getting stats
        {
           BuildLookup();
           
           float[] levels = lookupTable[characterClass][stat];
           
           if(levels.Length < level) // checks if the lenght of the level is smaller than the level, then returns 0
           {
               return 0;
           }

           return levels[level - 1]; // returning previous level's info to levels

        }

        public int GetLevels ( Stat stat, CharacterClass characterClass) // getting level info
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;

        }

        private void BuildLookup() // Look Up Build
        {
            if(lookupTable != null) return; // checks if its null then returns

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses) // traversing between classes
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats) // traversing between progression stat to progressionclass
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {

            public CharacterClass characterClass; // defining chracterclass
            //public float[] health; 
            public ProgressionStat[] stats; // defining progression stats

        }

        [System.Serializable] 
        class ProgressionStat 
        {
            public Stat stat;
            public float[] levels;
        }

    }

}