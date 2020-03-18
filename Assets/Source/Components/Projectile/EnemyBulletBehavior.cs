using UnityEngine;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Extensions;

namespace Assets.Source.Components.Projectile
{
    public class EnemyBulletBehavior : ProjectileComponentBase, IProjectileReactor
    {
        private readonly float MOVE_SPEED = 4.0f;

        protected override int BaseDamage => 20;

        public override void PerformStart()
        {
            RigidBody.velocity = -RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.PerformStart();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            Destroy(gameObject);
        }
    }
}