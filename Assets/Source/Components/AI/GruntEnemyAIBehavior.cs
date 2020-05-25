using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Components.Timer;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.AI
{
    /// <summary>
    /// Grunt enemies are very basic enemies a that are incredibly easy to destroy.  The max health should be kept under 
    /// the playerbullet's base damage (10) so that it gets destroyed in one shot.  
    /// </summary>
    public class GruntEnemyAIBehavior : ComponentBase, IProjectileReactor
    {
        private GameObject explosionObject;

        private IntervalTimerComponent intervalTimer;
        private GameObject bullet;

        private ActorBehavior actorBehavior;
        public override void ComponentAwake()
        {
            intervalTimer = GetRequiredComponent<IntervalTimerComponent>();
            intervalTimer.OnIntervalReached.AddListener(ShootIntervalReached);

            actorBehavior = GetRequiredComponent<ActorBehavior>();

            explosionObject = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Explosions/GruntExplosion");
            bullet = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/EnemyBullet");
            
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (actorBehavior.Health <= 0)
            {
                Explode();            
            }
            base.ComponentUpdate();
        }

        private void ShootIntervalReached()
        {
            InstantiateInLevel(bullet, transform.position.Copy(y: transform.position.y-0.25f));
        }

        // If the grunt enemy was shot:
        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            string collider = collision.otherCollider.gameObject.name;
            if (collider.Equals(GameObjects.Projectiles.PlayerBullet) || collider.Equals(GameObjects.Projectiles.Asteroid))
            {
                // Ideally This will result in a death after one shot
                actorBehavior.Health -= baseDamage;
            }
        }

        private void Explode() { 
            GameObject explosion = InstantiateInLevel(explosionObject, transform.position);
            explosion.transform.localScale = transform.localScale;
            Destroy(gameObject);
        
        }
    }
}
