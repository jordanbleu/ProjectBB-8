using Assets.Source.Components.Camera;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    public class PlayerBulletBehavior : ProjectileComponentBase
    {
        private readonly float MOVE_SPEED = 6f;

        protected override int BaseDamage => 10;

        private CameraEffectComponent cameraEffect;

        public override void ComponentAwake()
        {
            cameraEffect = GetRequiredComponent<CameraEffectComponent>(GetRequiredObject("PlayerVCam"));
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            RigidBody.velocity = RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.ComponentStart();
        }

        public override void ProjectileCollided(Collision2D collision)
        {
            // todo:  Eventaully to get the game to feel more satisfying we'll need to add impulse effects
            //cameraEffect.TriggerImpulse1();
            
            string colliderObjectName = collision.collider.name;
            string parentColliderObjectName = collision.collider.transform.parent?.name;

            if (!colliderObjectName.Equals(GameObjects.Actors.Player) && parentColliderObjectName != GameObjects.Actors.Player)
            {
                GameObject playerBulletExplosion = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/PlayerBulletExplosion");
                InstantiateInLevel(playerBulletExplosion, transform.position);
                Destroy(gameObject);
                base.ProjectileCollided(collision);
            }
        }

        
    }
}