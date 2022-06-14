// Fighter.cs file stands for arranging fighter's attributes in JaimGame

// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Movement;
using JAIM.Core;
using JAIM.Saving;
using JAIM.Attributes;
using JAIM.Stats;
using System.Collections.Generic;
using JAIM.Utils;
using System;

namespace JAIM.Combat // this namespace holds attributes about combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider // Monobehaviour is the base class for all created component in unity, also added other necessary coponents like Isavable
    {
        // SerializedField allow us to make a copy of our created variables in unity engine
        [SerializeField] float timeBetweenAttacks = 1f;  // defining there is a time between every attack
        [SerializeField] Transform rightHandTransform = null; // using transform defining righthand's transform for combat 
        [SerializeField] Transform leftHandTransform = null; // using transform defining lefthand's transform for combat
        [SerializeField] WeaponConfig defaultWeapon = null; // specifying a default weapon, when the game start player will have this default weapon

        Health target; 
        float passedTimeLastAttack = Mathf.Infinity;  
        WeaponConfig currentWeaponConfig; // defining current weapon's config
        LazyValue<Weapon> currentWeapon; // lazyvalue using when only calling

        private void Awake() // awake works only once when its called
        {
            // specifying current weapon is default weapon 
            currentWeaponConfig = defaultWeapon; 
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon); 
        }

        private Weapon SetupDefaultWeapon()
        {
            return GiveWeapon(defaultWeapon); // gives our player the default weapon
        }

        private void Start() // works when the game start to play
        {
            currentWeapon.ForceInit(); // initializing the current weapon
        }

        private void Update() // works once in every frame
        {
            passedTimeLastAttack += Time.deltaTime;

            if (target == null) return; // if there is no attack target return
            if (target.IsDead()) return; // if attack target is dead return, so we cant attack a dead enemy

            if (!CheckIfInRange(target.transform)) // this block of code checks our enemy is in the attack range or not
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f); // if enemy is not in player's attack range, player moves to a point that can attack to the enemy
            }
            else
            {
                GetComponent<Mover>().Cancel(); // if enemy is in the attack range of our player, our player stop moving
                AttackBehaviour(); // After stopping moving it calls attack behaviour for attacking selected target
            }
        }

        public void EquipWeapon(WeaponConfig weapon) // arranges equipped weapon configs
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = GiveWeapon(weapon);
        }

        private Weapon GiveWeapon(WeaponConfig weapon) // arranges selecting weapon in the game.
        {
            Animator animator = GetComponent<Animator>(); 
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator); // returning the new selected weapon, its spawning for hand transforms
        }

        public Health GetTarget() // arranges target's health
        {
            return target;
        }

        private void AttackBehaviour() // this is attack behaviour script in the game
        {
            transform.LookAt(target.transform); // checks the targets current tranform
            if (passedTimeLastAttack > timeBetweenAttacks) // checks if the time that passed we do last attack is greater than the time required between attacks
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                passedTimeLastAttack = 0;
            }
        }

        private void TriggerAttack() // Triggers the attack 
        {
            GetComponent<Animator>().ResetTrigger("stopAttack"); // shows in screen that attack is ended
            GetComponent<Animator>().SetTrigger("attack"); // shows in screen that attack is start
        }

        
        void Hit() // hit event
        {
            if (target == null) { return; } // checks if there is a target, if no it returns

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage); // Defining the damage value

            if (currentWeapon.value != null) // checks if the current weapon is not null, so we have a weapon then calls onhit method
            {
                currentWeapon.value.OnHit();
            }



            if (currentWeaponConfig.HasProjectile()) // checks if current weapon has a projectile (visual thing), then if it has launches it.
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else // if current weapon does not have a projectile it wont launch any visual thing
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot() // calls hit method
        {
            Hit();
        }

        private bool CheckIfInRange(Transform targetTransform) // this bool method takes care of if enemy is inside the weapon's range 
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public bool AbleToAttack(GameObject combatTarget) // this bool method takes care of if we can attack selected gameobject
        {
            if (combatTarget == null) { return false; } // if there is no target return null
            // checks if target is not in attack range and player cant move to this range and then returns false
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && 
                !CheckIfInRange(combatTarget.transform)) 
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget) // this code block stands for attack to targeted gameobject
        {
            GetComponent<ActionScheduler>().StartAction(this); // starts an action when we hit a gameobject
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() // this code block stands for when we dont want to continue attacking selected gameobject, it call stopattack function then makes target null
        {
            StopAttack(); // stoppes the attack 
            target = null; // makes target null
            GetComponent<Mover>().Cancel(); 
        }

        private void StopAttack() // stopping attack
        {
            GetComponent<Animator>().ResetTrigger("attack"); // This is stops attack animation
            GetComponent<Animator>().SetTrigger("stopAttack"); // This is stops stopattack animation
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) // IEnumerable using in foreach loops, foreach loops stands for traversing the elements of the array.
        // IEnumerable must be implemented for a class to gain iterator property.s
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage(); // yield return used when returning an element to the foreach loop that calls the Iterator
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) // IEnumerable must be implemented for a class to gain iterator property.
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus(); // yield return used when returning an element to the foreach loop that calls the Iterator
            }
        }

        public object CaptureState() // capturing the current state
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state) // restoring the current state
        {
            string weaponName = (string)state; 
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}