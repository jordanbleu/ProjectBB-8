using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    public class PlayerBulletBehavior : ProjectileComponentBase, IProjectileReactor
    {
        private readonly float MOVE_SPEED = 6f;

        protected override int BaseDamage => 10;

        public override void ComponentStart()
        {
            RigidBody.velocity = RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.ComponentAwake();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            Destroy(gameObject);
        }
    }
}