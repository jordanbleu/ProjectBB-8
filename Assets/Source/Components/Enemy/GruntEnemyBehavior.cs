using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Components.Timer;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using System;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    public class GruntEnemyBehavior : ProjectileComponentBase, IProjectileReactor
    {
        protected override int BaseDamage => 10;
        private GameObject explosionObject;

        private IntervalTimerComponent intervalTimer;
        private GameObject bullet;

        public override void ComponentAwake()
        {
            intervalTimer = GetRequiredComponent<IntervalTimerComponent>();
            intervalTimer.OnIntervalReached.AddListener(ShootIntervalReached);
            
            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/GruntExplosion");
            bullet = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/EnemyBullet");
            
            base.ComponentAwake();
        }

        private void ShootIntervalReached()
        {
            InstantiateInLevel(bullet, transform.position.Copy(y: transform.position.y-0.25f));
        }

        public override void DestroyProjectile(Collision2D collision)
        {
            if (!collision.collider.gameObject.name.Equals(bullet.name))
            {
                GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
                explosion.transform.localScale = transform.localScale;
                Destroy(gameObject);
            }
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if (!collision.otherCollider.gameObject.name.Equals(bullet.name))
            {
                GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
                explosion.transform.localScale = transform.localScale;
                Destroy(gameObject);
            }
        }

        
    }
}
