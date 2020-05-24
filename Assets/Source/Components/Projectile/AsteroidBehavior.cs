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

        private int spinSpeed;

        public override void ComponentStart()
        {
            System.Random random = new System.Random(DateTime.Now.Millisecond);
            RigidBody.velocity = new Vector2(random.NextFloatInRange(-0.5f, 0.5f), random.NextFloatInRange(-0.1f, -MAX_SPEED));
            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/AsteroidExplosion");

            float scale = UnityEngine.Random.Range(1f, 1.2f);
            transform.localScale = transform.localScale.Copy(x:scale, y:scale);

            spinSpeed = UnityEngine.Random.Range(-2, 2);

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            transform.Rotate(0,0, spinSpeed);
            base.ComponentUpdate();
        }

        public override void ProjectileCollided(Collision2D collision)
        {
            GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
            
            // Scale size of the explosion to be the same as the asteroids scale

            float scale = (transform.localScale.x / 5);

            // Todo: if we need to re-use this functionality, we should probably move this logic into a new debris component or somethin
            foreach (ParticleSystem particleSystem in explosion.GetComponentsInChildren<ParticleSystem>())
            {
                if (particleSystem.gameObject.name.Equals("Debris"))
                {
                    var mainModule = particleSystem.main;
                    mainModule.startSize = new ParticleSystem.MinMaxCurve(scale);
                }
            
            }
            
            explosion.transform.localScale = transform.localScale;
            
            Destroy(gameObject);
            // No need to call base since we already destroyed the object here
        }
    }
}
