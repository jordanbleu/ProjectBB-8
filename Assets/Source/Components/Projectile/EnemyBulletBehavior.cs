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
        private GameObject explosionPrefab;

        public override void ComponentAwake()
        {
            explosionPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/EnemyBulletExplosion");            

            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            RigidBody.velocity = -RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.ComponentStart();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            string collisionName = collision.otherCollider.gameObject.transform.parent.name;

            if (collisionName.Equals(GameObjects.Actors.Player))
            {
                InstantiateInLevel(explosionPrefab, transform.position);
                Destroy(gameObject);
            }
        }
    }
}