using System;
using Assets.Source.Components.Enemy.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    /// <summary>
    /// This enemy doesn't shoot at the player but charges straight at the player trying to smash into them for max damage. It will not
    /// dodge other obstacles or do anything else advanced, but it will rotate to face the enemy at all times.
    /// </summary>
    public class KamikazeEnemyBehavior : EnemyAIBase, IProjectileReactor
    {
        private AudioClip explosionSound;
        private AudioSource audioSource;


        private readonly float MOVE_SPEED = 1.35f;
        private readonly float MOVEMENT_THRESHOLD = 0.025f; //how close the enemy needs to be to the player before it will stop moving, can't be 0
        private readonly float STUN_DURATION = 0.5f; //the time after being hit by a projectile in which the enemy is stunned

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

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            audioSource.PlayOneShot(explosionSound);

            if (collision.otherCollider.gameObject.name.Equals(GameObjects.Player))
            {
                actorBehavior.Health = 0;
            }
            else
            {
                actorBehavior.Health -= baseDamage;
            }

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