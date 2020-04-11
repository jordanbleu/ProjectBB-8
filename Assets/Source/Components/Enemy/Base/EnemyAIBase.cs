using UnityEngine;
using Assets.Source.Constants;
using Assets.Source.Components.Actor;
using Assets.Source.Extensions;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;

namespace Assets.Source.Components.Enemy.Base
{
    /// <summary>
    /// The base shared logic between all enemies. Also turns an enemy ship into a "projectile" so that if the player
    /// runs into an enemy they will collide
    /// </summary>
    public abstract class EnemyAIBase : ProjectileComponentBase, IProjectileReactor
    {
        private readonly float STABILIZATION_RATE = 0.01f;

        protected GameObject explosionPrefab;
        protected Vector2 externalVelocity;
        protected Rigidbody2D rigidBody;
        protected ActorBehavior actorBehavior;
        protected GameObject player;

        public override void ComponentAwake()
        {
            player = GetRequiredObject(GameObjects.Actors.Player);
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            base.ComponentAwake();
        }

        /// <summary>
        /// Checks the health of the enemy, then creates an explosion and destroyes self after health reaches 0
        /// </summary>
        public virtual void UpdateActorStatus()
        {
            if (actorBehavior.Health <= 0)
            {
                InstantiateInLevel(explosionPrefab, transform.position);
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Gets the current transform position of the player
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetPlayerLocation()
        {
            if (player != null)
            { 
                return player.transform.position;
            }
            return Vector3.zero; //todo: fix this
        }

        protected void AvoidOtherEnemies()
        {

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

        public abstract void ReactToHit(Collision2D collision, int baseDamage);

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            ReactToHit(collision, baseDamage);
        }

        public abstract void ReactToProjectileCollision(Collision2D collision);

        public override void DestroyProjectile(Collision2D collision)
        {
            ReactToProjectileCollision(collision);
        }
    }
}