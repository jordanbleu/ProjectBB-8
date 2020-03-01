using Assets.Source.Components.Projectile.Base;
using Assets.Source.Extensions;
using System;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    public class AsteroidBehavior : ProjectileComponentBase
    {
        private readonly float MAX_SPEED = 1f;

        protected override int BaseDamage => 5;

        public override void Create()
        {
            System.Random random = new System.Random(DateTime.Now.Millisecond);
            RigidBody.velocity = new Vector2(random.NextFloatInRange(-0.5f, 0.5f), random.NextFloatInRange(-0.1f, -MAX_SPEED));

            base.Create();
        }

        public override void DestroyProjectile(Collision2D collision)
        {
            Debug.Log($"Asteroid Collided with {collision.gameObject} and was destroyed");
            Destroy(gameObject);
            base.DestroyProjectile(collision);
        }
    }
}
