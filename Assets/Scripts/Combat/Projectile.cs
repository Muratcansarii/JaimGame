// Projectile.cs file stands for arranging attributes of projectiles in JaimGame


// Adding namespaces that we keep in safe 
using UnityEngine;
using JAIM.Attributes;
using UnityEngine.Events;

namespace JAIM.Combat // this namespace holds attributes about combat
{

    public class Projectile : MonoBehaviour// Monobehaviour is the base class for all created component in unity
    {
        // SerializedField allow us to make a copy of our created variables in unity engine,
        [SerializeField] float speed = 1; 
        [SerializeField] bool isHoming = true; 
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10; 
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;

        Health target = null; // first target is null in the game
        GameObject instigator = null; 
        float damage = 0; 

        private void Start() // works when the game start to play
        {
            transform.LookAt(GetAimLocation()); // transform defines location of the aimed target
        }



        void Update() // works once in every frame
        {
            if (target == null) return; // if there is no target return
            if (isHoming && !target.IsDead()) // if target is not dead, take the location of the aimed target
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime); // calculates the moving behaviour using transform
        }

        public void SetTarget(Health target,GameObject instigator, float damage) // this method responsible specifying the setting the target
        {
            this.target = target; 
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        // A piece of code that works to make the right direction to the torso of the enemy, not the feet, 
        //when an attack is made with a weapon such as an arrow. Capsule Height is divided into 2, so it looks like the full body has been hit.
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            // each object may not be a capsule, if the target does not have a capsule, the arrow will go directly according to the positions
            if (targetCapsule == null) 
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.GetComponent<Health>() != target) return; // prevent decreasing health of unselected enemies
            if (target.IsDead()) return; // if target is dead return
            target.TakeDamage(instigator, damage);

            speed = 0;

            onHit.Invoke();
            
            if (hitEffect != null) // check if hiteffect is not nul, then play the effect
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
