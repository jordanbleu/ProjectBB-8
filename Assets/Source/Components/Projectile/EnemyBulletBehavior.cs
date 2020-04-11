using UnityEngine;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Extensions;
using Assets.Source.Constants;

namespace Assets.Source.Components.Projectile
{
    public class EnemyBulletBehavior : ProjectileComponentBase, IProjectileReactor
    {
        private readonly float MOVE_SPEED = 4.0f;

        protected override int BaseDamage => 20;

        public override void ComponentStart()
        {
            RigidBody.velocity = -RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.ComponentStart();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            string collisionName = collision.otherCollider.gameObject.name;
            if (!collisionName.Equals(GameObjects.Actors.ShooterEnemy) && !collisionName.Equals(GameObjects.Actors.KamikazeEnemy))
            {
                Destroy(gameObject);
            }
        }
    }
}