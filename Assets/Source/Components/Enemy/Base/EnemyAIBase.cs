﻿using UnityEngine;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using System;
using Assets.Source.Components.Actor;
using Assets.Source.Extensions;

namespace Assets.Source.Components.Enemy.Base
{
    public abstract class EnemyAIBase : ComponentBase
    {
        protected GameObject player;
        
        protected Rigidbody2D rigidBody;
        protected ActorBehavior actorBehavior;
        private readonly float STABILIZATION_RATE = 0.01f;

        private GameObject explosionPrefab;
        protected Vector2 externalVelocity;

        public override void PerformAwake()
        {
            //todo: design a better way to have a reference to the play since every spawned enemy will have to try and "find" the player
            player = GetRequiredObject(GameObjects.Player);
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            
            base.PerformAwake();
        }

        /// <summary>
        /// Should be overriden to add logic in how the enemy will act, given various game inputs
        /// </summary>
        public virtual void UpdateActorBehavior() { }

        /// <summary>
        /// Checks the health of the enemy, then creates an explosion and destroyes self after health reaches 0
        /// </summary>
        public virtual void UpdateActorStatus()
        {
            if (actorBehavior.Health <= 0)
            {
                Debug.Log("Enemy Destroyed");
                InstantiatePrefab(explosionPrefab, transform.position);
                Destroy(gameObject);
            }
        }

        protected Vector3 GetPlayerLocation()
        {
            Vector3 playerLocation = new Vector3();
            if (player != null)
            {
                playerLocation = player.transform.position;
            }
            return playerLocation;
        }

        protected void UpdateExternalVelocity()
        {
            if (externalVelocity.x > 0)
            {
                externalVelocity = externalVelocity.Copy(x: externalVelocity.x - STABILIZATION_RATE);
            }
            else if (externalVelocity.x < 0)
            {
                externalVelocity = externalVelocity.Copy(x: externalVelocity.x + STABILIZATION_RATE);
            }

            if (externalVelocity.y > 0)
            {
                externalVelocity = externalVelocity.Copy(y: externalVelocity.y - STABILIZATION_RATE);
            }
            else if (externalVelocity.y < 0)
            {
                externalVelocity = externalVelocity.Copy(y: externalVelocity.y + STABILIZATION_RATE);
            }

            // Prevents overshoot
            if (externalVelocity.x.IsWithin(STABILIZATION_RATE, 0f)) { externalVelocity = externalVelocity.Copy(x: 0f); }
            if (externalVelocity.y.IsWithin(STABILIZATION_RATE, 0f)) { externalVelocity = externalVelocity.Copy(y: 0f); }
        }
    }
}