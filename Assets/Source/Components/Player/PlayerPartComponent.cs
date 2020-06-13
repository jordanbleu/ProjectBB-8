using Assets.Source.Components.Actor;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerPartComponent : ProjectileComponentBase, IProjectileReactor
    {
        [SerializeField]
        private float partDamageMultiplier = 1;

        private PlayerBehavior playerBehavior;

        protected override int BaseDamage => 10;

        public override void ComponentAwake()
        {
            playerBehavior = GetRequiredComponentInParent<PlayerBehavior>();
            base.ComponentAwake();
        }

        /// <summary>
        /// This is called when the player gets hit by something that is a projectile, such as a bullet, asteroid, etc
        /// </summary>
        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            // If the player gets hit by a projectile, projectile inflicts damage
            if (!collision.otherCollider.name.Equals(GameObjects.Projectiles.PlayerBullet))
            {
                playerBehavior.ReactToHit(collision, baseDamage, partDamageMultiplier);
            }
        }

        /// <summary>
        /// This gets called when the player is hit (or hits) something that is not a projectile, such as another enemy
        /// </summary>
        public override void ProjectileCollided(Collision2D collision)
        {
            // If the colliding object is a projectile, let the ReactToProjectileHit handle it
            if (!collision.collider.TryGetComponent<ProjectileComponentBase>(out _)) { 
                playerBehavior.ReactToHit(collision, BaseDamage, partDamageMultiplier);
            }

            // If the colliding object is an actor, apply damage to it.  This makes ramming enemies a viable option!
            if (collision.collider.TryGetComponent(out ActorBehavior actor)) {
                actor.Health -= BaseDamage;
            }

            base.ProjectileCollided(collision);
        }
    }
}
