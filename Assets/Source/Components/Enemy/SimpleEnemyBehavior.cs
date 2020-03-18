using System;
using Assets.Source.Components.Enemy.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    /// <summary>
    /// This enemy 
    /// </summary>
    public class SimpleEnemyBehavior : EnemyAIBase, IProjectileReactor
    {
        private GameObject enemyBulletPrefab;
        private Vector2 distanceToPlayer;
        private float timeUntilNextShot;
        private readonly float MOVE_SPEED = 2.0f;
        private readonly float FIRE_INTERVAL = 1.5f;
        private readonly float FIRE_RANGE = .6f;
        private readonly float MOVEMENT_THRESHOLD = 0.01f; //how close the enemy needs to be to the player before it will stop moving, can't be 0
        private readonly float STUN_TIME = 0.4f;
        private float currentStunTime = 0.0f;

        public override void PerformAwake()
        {
            enemyBulletPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Projectiles/{GameObjects.EnemyBullet}");
            timeUntilNextShot = FIRE_INTERVAL;
            base.PerformAwake();
        }

        public override void PerformStart()
        {
            distanceToPlayer = GetPlayerLocation() - transform.position;
            base.PerformStart();
        }

        public override void PerformUpdate()
        {
            UpdateActorBehavior();
            UpdateActorStatus();
            UpdateExternalVelocity();
            base.PerformUpdate();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if (!collision.otherCollider.gameObject.name.Equals(enemyBulletPrefab.name))
            {
                actorBehavior.Health -= baseDamage;
                currentStunTime = STUN_TIME;

                // Hit from the left side
                if (collision.otherCollider.transform.position.x <= transform.position.x)
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x + .4f);
                }
                // Hit from the right side
                else
                {
                    externalVelocity = externalVelocity.Copy(x: externalVelocity.x - .4f);
                }

                // Hit from the ass
                if (collision.otherCollider.transform.position.y <= transform.position.y)
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.x + .4f);
                }
                // Hit from the right side
                else
                {
                    externalVelocity = externalVelocity.Copy(y: externalVelocity.y - .4f);
                }
            }
        }

        public override void UpdateActorBehavior()
        {
            if(currentStunTime > 0)
            {
                currentStunTime -= Time.deltaTime;
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

                //float horizontalMoveDelta = Vector2.Lerp(Vector2.zero, horizontalMoveDirection, linearInterpolate).x * MOVE_SPEED;
                //float verticalMoveDelta = Vector2.Lerp(Vector2.zero, verticalMoveDirection, linearInterpolate).y * MOVE_SPEED;

                //float distanceXSpeed = SafeGetPercentage(Math.Abs(locationDifference.x), MOVE_SPEED) * MOVE_SPEED;
                //float distanceYSpeed = SafeGetPercentage(Math.Abs(locationDifference.y), MOVE_SPEED) * MOVE_SPEED;

                float sqrtDistanceXSpeed = SqrtSpeedCalc(Math.Abs(locationDifference.x));
                float sqrtDistanceYSpeed = SqrtSpeedCalc(Math.Abs(locationDifference.y));

                float expDistanceXSpeed = ExpSpeedCalc(Math.Abs(locationDifference.x));
                float expDistanceYSpeed = ExpSpeedCalc(Math.Abs(locationDifference.y));

                float horizontalMoveDelta = horizontalMoveDirection.x * expDistanceXSpeed;
                float verticalMoveDelta = verticalMoveDirection.y * expDistanceYSpeed;

                Vector2 prevVel = rigidBody.velocity;
                rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta) + externalVelocity;
                if(prevVel != rigidBody.velocity)
                {
                    Debug.Log($"Velocity: {rigidBody.velocity}");
                }

                if (ShouldFire())
                {
                    GameObject bullet = InstantiatePrefab(enemyBulletPrefab);
                    bullet.transform.position = transform.position.Copy();
                }
            }
        }

        private bool ShouldFire()
        {
            bool shouldFire = false;
            Vector3 playerLocation = GetPlayerLocation();
            //fire if player is within horizontal range
            if (Math.Abs(playerLocation.x) < Math.Abs(transform.position.x) + FIRE_RANGE)
            {
                timeUntilNextShot -= Time.deltaTime;
                if (timeUntilNextShot < 0)
                {
                    timeUntilNextShot = FIRE_INTERVAL;
                    shouldFire = true;
                }
            }

            return shouldFire;
        }

        private float SafeGetPercentage(float numerator, float denominator = 100f)
        {
            if(denominator == 0)
            {
                throw new ArgumentException("Value cannot be 0", "denominator");
            }
            else if(numerator == 0)
            {
                return 0f;
            }
            else if(numerator > denominator)
            {
                return 1f;
            }
            else
            {
                return numerator / denominator;
            }
        }

        private float SqrtSpeedCalc(float currentDistance)
        {
            double sqrtDistance = Math.Sqrt((double)currentDistance);
            return Mathf.Clamp((float)sqrtDistance, 0, MOVE_SPEED);
        }

        private float ExpSpeedCalc(float currentDistance)
        {
            float expDistance = Mathf.Exp(currentDistance) - 0.65f;
            return Mathf.Clamp(expDistance, 0, MOVE_SPEED);
        }
    }
}