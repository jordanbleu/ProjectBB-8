using Assets.Source.Components.Base;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    public class GruntEnemyBehavior : ProjectileComponentBase, IProjectileReactor
    {
        protected override int BaseDamage => 10;
        private GameObject explosionObject;

        public override void ComponentAwake()
        {
            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/GruntExplosion");
            base.ComponentAwake();
        }

        public override void DestroyProjectile(Collision2D collision)
        {
            GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
            explosion.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
            explosion.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
    }
}
