using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
    public abstract class GunAmmoBehaviour : MonoBehaviourPun
    {
        [HideInInspector] public SpacecraftController owner = null;

        public string weaponName = "Gun";
        [SerializeField] protected float destroyTime = 5;
        [SerializeField] protected GameObject impactFX;
        [SerializeField] protected float speed = 1000;
        [SerializeField] protected float colliderDelay = 0.01f;
        public float damageOutput = 223;
        
        public float speedModifier;
        bool canDamageOwner = false;
        protected Collider thisCollider;

        private void OnDisable() {

            canDamageOwner = false;

            
        }

    }
}
