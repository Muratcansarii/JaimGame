// WeaponConfig.cs file stands for arranging configs of weapons in JaimGame

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Attributes;

namespace JAIM.Combat // this namespace holds attributes about combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)] // creating a ui tool that stands for creating a new weapon menu
    public class WeaponConfig : ScriptableObject // this indicates that there is a unity object that has a script attribute
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null; // In first phase of the game, equipped weapon is null
        [SerializeField] float weaponDamage = 5f; // defining the weapon's damage
        [SerializeField] float percentBonus = 0; 
        [SerializeField] float weaponRange = 2f; // defining the weapon's range
        [SerializeField] bool isRightHanded = true; // checking if the weapon equipped in right hand or left hand
        [SerializeField] Projectile projectile = null; 

        const string weaponName = "Weapon"; // defining a permanent constant 

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator) // this block of code arranges the weapon behaviour of spawning in the game
        {
            RemoveOldWeapon(rightHand, leftHand); // for spawning a new weapon first old one should be destroyed
            Weapon weapon = null;  // first make weapon null

            if (equippedPrefab != null)  // check if there is no equipped weapon
            {
                Transform handTransform = GetTransform(rightHand, leftHand); 
                weapon = Instantiate(equippedPrefab, handTransform); // takes the equipped weapon's transform
                weapon.gameObject.name = weaponName; // takes the name of the weapon
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void RemoveOldWeapon(Transform rightHand, Transform leftHand) // this block of code responsible for removing the old weapon
        {
            Transform oldWeapon = rightHand.Find(weaponName); // getting information of oldweapons transform
            if (oldWeapon == null) // if there is no oldweapon
            {
                oldWeapon = leftHand.Find(weaponName); // use the weapon of player uses
            }
            if (oldWeapon == null) return; // if our player has no weapon in his hands then return

            oldWeapon.name = "DESTROYING"; // print the console that old weapon is destroying
            Destroy(oldWeapon.gameObject); // destroy the old weapon game object
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand) // this block of code responsible for getting the transform of the weapon
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand; // checks the weapon is right handed or not
            else handTransform = leftHand; // if not right handed so its left handed
            return handTransform; // returning headtransform
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        // this block of code responsible for launching projectile
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) 
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage() // returns the damage of the weapon
        {
            return weaponDamage;
        }

        public float GetPercentageBonus() // returns the percantage bonus
        {
            return percentBonus;
        }

        public float GetRange() // returns the range of the weapon
        {
            return weaponRange;
        }
    }
}