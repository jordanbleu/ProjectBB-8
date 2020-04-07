using System;
using Assets.Source.Components.Enemy.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    /// <summary>
    /// This enemy is basic enemy that will shoot at the player when close enough and shoots at a regular interval,
    /// it also gets stunned when hit by a projectile and has relatively low health.
    /// </summary>
    public class SimpleEnemyBehavior : EnemyAIBase, IProjectileReactor
    {
        private readonly float MOVE_SPEED = 2.0f;
        private readonly float SHOOT_COOLDOWN = 1.5f; //the amount of time allowed between firing shots
        private readonly float SHOOT_THRESHOLD = .6f; //how close the enemy needs to be to the player to shoot
        private readonly float MOVEMENT_THRESHOLD = 0.01f; //how close the enemy needs to be to the player before it will stop moving, can't be 0
        private readonly float STUN_DURATION = 0.4f; //the time after being hit by a projectile in which the enemy is stunned

        private GameObject enemyBulletPrefab;
        private Vector2 distanceToPlayer;
        private float currentShootCooldown;
        private float currentStunCooldown = 0.0f;

        private AudioSource audioSource;
        private AudioClip explosionSound;
        private AudioClip blasterSound;

        protected override int BaseDamage => 30;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            explosionSound = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/Explosion/smallImpact");
            blasterSound = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/Enemy/enemyBlaster");

            enemyBulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.EnemyBullet}");
            currentShootCooldown = SHOOT_COOLDOWN;
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            distanceToPlayer = GetPlayerLocation() - transform.position;
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            base.UpdateActorStatus();
            UpdateActorBehavior();
            UpdateExternalVelocity();
            base.ComponentUpdate();
        }

        private void UpdateActorBehavior()
        {
            if(currentStunCooldown > 0.0f)
            {
                currentStunCooldown -= Time.deltaTime;
                rigidBody.velocity = rigidBody.velocity.Copy(0f, 0f) + externalVelocity;
            }
            else
            {
                Vector2 updatedDistanceToPlayer = GetPlayerLocation() - transform.position;
                Vector2 locationDifference = updatedDistanceToPlayer - distanceToPlayer;
                Vector2 horizontalMoveDirection = Vector2.zero;
                Vector2 verticalMoveDirection = Vector2.zero;

                if (locationDifference.x > 0 + MOVEMENT_THRESHOLD)
                {
                    horizontalMoveDirection = Vector2.right;
                }
                else if (locationDifference.x < 0 - MOVEMENT_THRESHOLD)
                {
                    horizontalMoveDirection = Vector2.left;
                }
                if (locationDifference.y < 0 - MOVEMENT_THRESHOLD)
                {
                    verticalMoveDirection = Vector2.down;
                }
                else if (locationDifference.y > 0 + MOVEMENT_THRESHOLD)
                {
                    verticalMoveDirection = Vector2.up;
                }

                float horizontalMoveDelta = horizontalMoveDirection.x * AdjustSpeedExponential(Math.Abs(locationDifference.x));
                float verticalMoveDelta = verticalMoveDirection.y * AdjustSpeedExponential(Math.Abs(locationDifference.y));

                rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta) + externalVelocity;

                if (ShouldShoot())
                {
                    audioSource.PlayOneShot(blasterSound);
                    GameObject bullet = InstantiateInLevel(enemyBulletPrefab);
                    bullet.transform.position = transform.position.Copy();
                }
            }
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            audioSource.PlayOneShot(explosionSound);

            if (!collision.otherCollider.gameObject.name.Equals(enemyBulletPrefab.name))
            {
                actorBehavior.Health -= baseDamage;
                currentStunCooldown = STUN_DURATION;

                //since the enemy can't respond to being hit like the player can, reduce the impact
                // Hit from the left side
                if (collision.otherCollider.transform.position.x <= transform.position.x)
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x + .3f);
                }
                // Hit from the right side
                else
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x - .3f);
                }

                // Hit from the ass
                if (collision.otherCollider.transform.position.y <= transform.position.y)
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.x + .3f);
                }
                // Hit from the right side
                else
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.y - .3f);
                }
            }
        }

        private bool ShouldShoot()
        {
            bool shouldFire = false;
            Vector3 playerLocation = GetPlayerLocation();

            //always cool down the shoot timer but only shoot once close enough
            currentShootCooldown -= Time.deltaTime;
            if (currentShootCooldown < 0 && Math.Abs(playerLocation.x) < Math.Abs(transform.position.x) + SHOOT_THRESHOLD)
            {
                currentShootCooldown = SHOOT_COOLDOWN;
                shouldFire = true;
            }

            return shouldFire;
        }

        /// <summary>
        /// Uses an exponential equation to calculate how fast the enemy should move towards the player.
        /// This helps give the enemy a more realistic feel to it as it will catch up fast but slow down the approach once closer to the player.
        /// </summary>
        /// <param name="currentDistance">The current distace from the player</param>
        /// <returns></returns>
        private float AdjustSpeedExponential(float currentDistance)
        {
            float expDistance = Mathf.Exp(currentDistance) - 0.65f;
            return Mathf.Clamp(expDistance, 0, MOVE_SPEED);
        }
    }
}