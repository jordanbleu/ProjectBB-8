using Assets.Source.Components.Projectile.Base;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using System;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    public class AsteroidBehavior : ProjectileComponentBase
    {
        private readonly float MAX_SPEED = 1f;

        protected override int BaseDamage => 5;

        private GameObject explosionObject;

        public override void ComponentStart()
        {
            System.Random random = new System.Random(DateTime.Now.Millisecond);
            RigidBody.velocity = new Vector2(random.NextFloatInRange(-0.5f, 0.5f), random.NextFloatInRange(-0.1f, -MAX_SPEED));
            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/Explosion_2");

            float scale = UnityEngine.Random.Range(1f, 3f);
            transform.localScale = transform.localScale.Copy(x:scale, y:scale);

            base.ComponentStart();
        }

        public override void DestroyProjectile(Collision2D collision)
        {
            Debug.Log($"Asteroid Collided with {collision.gameObject} and was destroyed");
            InstantiateLevelPrefab(explosionObject, transform.position);
            Destroy(gameObject);
            // No need to call base since we already destroyed the object here
        }
    }
}
