using Assets.Source.Components.Base;
using Assets.Source.Components.Reactor.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Projectile.Base
{
    /// <summary>
    /// This class automatically handles memory management and other things for all projectiles
    /// </summary>
    public abstract class ProjectileComponentBase : ComponentBase
    {
        /// <summary>
        /// Determines the base amount of damage this object inflicts when colliding with something
        /// </summary>
        protected abstract int BaseDamage { get; }

        protected Rigidbody2D RigidBody { get; private set; }


        public override void ComponentAwake()
        {
            RigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentAwake();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // if the collided gameobject has a collider reactor, tell it to react
            if (collision.gameObject.TryGetComponent(out IProjectileReactor reactor))
            {
                reactor.ReactToProjectileHit(collision, BaseDamage);
            }

            // Projectiles usually destroy themselves on contact but not always
            ProjectileCollided(collision);
        }

        public virtual void ProjectileCollided(Collision2D collision) { }
    }
}
