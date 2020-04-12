using System;
using Assets.Source.Components.Enemy.Base;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    /// <summary>
    /// This enemy doesn't shoot at the player but charges straight at the player trying to smash into them for max damage. It will not
    /// dodge other obstacles or do anything else advanced, but it will rotate to face the enemy at all times.
    /// </summary>
    public class KamikazeEnemyBehavior : EnemyAIBase
    {
        private readonly float MOVE_SPEED = 1.35f;
        private readonly float MOVEMENT_THRESHOLD = 0.025f; //how close the enemy needs to be to the player before it will stop moving, can't be 0

        // Audio
        private AudioClip explosionSound;
        private AudioSource audioSource;

        private float currentStunCooldown = 0.0f;

        protected override int BaseDamage => 50;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            explosionSound = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/Explosion/smallImpact");

            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            base.UpdateActorStatus();
            LookAtPlayer();
            UpdateActorBehavior();
            UpdateExternalVelocity();
            base.ComponentUpdate();
        }

        public override void ReactToHit(Collision2D collision, int baseDamage)
        {
            audioSource.PlayOneShot(explosionSound);
            //we don't need to do anything here since kamikaze dies on impact of anything
            //if we want to make it so that this enemy can take multiple hits from the players bullets
            //then we should ignore collisions from player bullets in ReactToProjectileCollision
            //then add the logic back in here to reduce the health of this enemy and knock them back from the impact
        }

        public override void ReactToProjectileCollision(Collision2D collision)
        {
            InstantiatePrefab(explosionPrefab, transform.position);
            Destroy(gameObject);
        }

        private void LookAtPlayer()
        {
            // todo: fix this
            if (player != null)
            {
                transform.up = player.transform.position - transform.position;
            }
        }

        private void UpdateActorBehavior()
        {
            if (currentStunCooldown > 0.0f)
            {
                currentStunCooldown -= Time.deltaTime;
                rigidBody.velocity = rigidBody.velocity.Copy(0f, 0f) + externalVelocity;
            }
            else
            {
                Vector2 updatedDistanceToPlayer = GetPlayerLocation() - transform.position;
                Vector2 horizontalMoveDirection = Vector2.zero;
                Vector2 verticalMoveDirection = Vector2.zero;

                if (updatedDistanceToPlayer.x > 0 + MOVEMENT_THRESHOLD)
                {
                    horizontalMoveDirection = Vector2.right;
                }
                else if (updatedDistanceToPlayer.x < 0 - MOVEMENT_THRESHOLD)
                {
                    horizontalMoveDirection = Vector2.left;
                }
                if (updatedDistanceToPlayer.y < 0 - MOVEMENT_THRESHOLD)
                {
                    verticalMoveDirection = Vector2.down;
                }
                else if (updatedDistanceToPlayer.y > 0 + MOVEMENT_THRESHOLD)
                {
                    verticalMoveDirection = Vector2.up;
                }

                float horizontalMoveDelta = horizontalMoveDirection.x * AdjustSpeedExponential(Math.Abs(updatedDistanceToPlayer.x));
                float verticalMoveDelta = verticalMoveDirection.y * AdjustSpeedExponential(Math.Abs(updatedDistanceToPlayer.y));

                rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta) + externalVelocity;
            }
        }

        /// <summary>
        /// Uses an exponential equation to calculate how fast the enemy should move towards the player.
        /// This helps give the enemy a more realistic feel to it as it will catch up fast but slow down the approach once closer to the player.
        /// </summary>
        /// <param name="currentDistance">The current distace from the player</param>
        /// <returns></returns>
        private float AdjustSpeedExponential(float currentDistance)
        {
            float expDistance = Mathf.Exp(currentDistance) - 0.4f;
            return Mathf.Clamp(expDistance, 0, MOVE_SPEED);
        }
    }
}