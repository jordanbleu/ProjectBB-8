using UnityEngine;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Components.Actor;
using Assets.Source.Constants;

namespace Assets.Source.Components.Enemy.Base
{
    public class BaseEnemyBehavior : EnemyAIBehavior, IProjectileReactor
    {
        protected Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;

        protected GameObject enemyBulletPrefab;
        private GameObject explosionPrefab;

        public override void Construct()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();
            enemyBulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.EnemyBullet}");
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_1");

            base.Construct();
        }

        /// <summary>
        /// Should be overriden to add logic in how the enemy will act, given various input
        /// </summary>
        public virtual void UpdateActorBehavior() { }

        /// <summary>
        /// Reacts to being hit by another collider by reducing health
        /// </summary>
        /// <param name="collision">The collision object of whatever this has collided with</param>
        /// <param name="baseDamage"></param>
        public virtual void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if(!collision.otherCollider.gameObject.name.Equals(enemyBulletPrefab.name))
            {
                actorBehavior.Health -= baseDamage;
            }
        }

        /// <summary>
        /// Checks the health of the enemy, then creates an explosion and destroyes self after health reaches 0
        /// </summary>
        public virtual void UpdateActorStatus()
        {
            if(actorBehavior.Health <= 0)
            {
                InstantiatePrefab(explosionPrefab, transform.position);
                Destroy(gameObject);
            }
        }
    }
}