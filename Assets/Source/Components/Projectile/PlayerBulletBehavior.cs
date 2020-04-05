using Assets.Source.Components.Camera;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    public class PlayerBulletBehavior : ProjectileComponentBase, IProjectileReactor
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

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            Destroy(gameObject);
        }

        public override void ComponentOnDestroy()
        {
            // todo:  Eventaully to get the game to feel more satisfying we'll need to add impulse effects
            //cameraEffect.TriggerImpulse1(); 
            GameObject playerBulletExplosion = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/PlayerBulletExplosion");
            InstantiateLevelPrefab(playerBulletExplosion, transform.position);
            base.ComponentOnDestroy();
        }
    }
}