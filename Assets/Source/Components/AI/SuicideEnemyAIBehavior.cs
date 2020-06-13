using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Timer;
using Assets.Source.Extensions;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.AI
{
    [RequireComponent(typeof(AudioSource), typeof(Rigidbody2D), typeof(Animator))]
    public class SuicideEnemyAIBehavior : ProjectileComponentBase
    {
        public enum BlinkSpeed { 
            Slow = 0,
            Medium = 1,
            Fast = 2,
            VeryFast = 3
        }

        [Header("Behavior Setup")]
        [Tooltip("The amount of base damage the enemy inflcits on collision")]
        [SerializeField]
        private int baseDamage = 5;

        [Tooltip("Maximum movement speed")]
        [SerializeField]
        private float moveSpeed = 2f;

        [Tooltip("How long before the enemy exlodes on their own (milliseconds)")]
        [SerializeField]
        private float lifetime = 6000;

        [Header("Object References")]
        [Tooltip("The explosion prefab to instantiate on death.  Can be left null")]
        [SerializeField]
        private GameObject explosionPrefab;

        [Header("Audio Clips")]
        [Tooltip("The sound to play on death.  Can be left null")]
        [SerializeField]
        private AudioClip deathSound;

        // Constants
        private readonly float MOVEMENT_THRESHOLD = 0.01f;

        // Components
        private AudioSource audioSource;
        private Rigidbody2D rigidBody;
        private Animator animator;

        // Fields
        private GameObject currentTarget;
        private IntervalTimerComponent lifeTimer;
        private BlinkSpeed lightBlinkingSpeed = BlinkSpeed.Slow;

        // Damage when I collide with the target
        protected override int BaseDamage => baseDamage;

        public override void ComponentAwake()
        {
            CreateTimers();
            audioSource = GetRequiredComponent<AudioSource>();
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        private void CreateTimers()
        {
            lifeTimer = gameObject.AddComponent<IntervalTimerComponent>();
            lifeTimer.SetInterval(lifetime);
            lifeTimer.AutoReset = false;
            lifeTimer.Randomize = false;
            lifeTimer.IsActive = true;
            lifeTimer.OnIntervalReached.AddListener(Die);
        }

        public override void ComponentStart()
        {
            currentTarget = ChooseNextTarget();

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            FaceTarget();
            MoveTowardsPosition();
            CheckLifeTimer();
            UpdateAnimator();
            base.ComponentUpdate();
        }

        private void UpdateAnimator()
        {
            animator.SetInteger("blink_speed", (int)lightBlinkingSpeed);
        }

        private void CheckLifeTimer()
        {
            float percentage = lifeTimer.CurrentTime / lifetime;

            if (percentage < 0.25)
            {
                lightBlinkingSpeed = BlinkSpeed.Slow;
            }
            else if (percentage < 0.5)
            {
                lightBlinkingSpeed = BlinkSpeed.Medium;
            }
            else if (percentage < 0.75)
            {
                lightBlinkingSpeed = BlinkSpeed.Fast;
            }
            else
            {
                lightBlinkingSpeed = BlinkSpeed.VeryFast;
            }            
        }

        private void MoveTowardsPosition()
        {
            // if the current target is still alive, seek them out, otherwise just dive bomb towards valhalla
            Vector2 destination = currentTarget?.transform?.position ?? new Vector2(transform.position.x, 6);

            var locationDifference = destination - transform.position.ToVector2();

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

            rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta);
        }

        private void FaceTarget()
        {
            if (currentTarget != null)
            {
                Vector3 currentPosition = transform.position;
                currentPosition.z = 0f;

                Vector3 targetPosition = currentTarget.transform.position;
                currentPosition.x -= targetPosition.x;
                currentPosition.y -= targetPosition.y;

                float angle = 90+Mathf.Atan2(currentPosition.y, currentPosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        // Choose from the active IFriendlyTargets in the scene 
        private GameObject ChooseNextTarget()
        {
            ComponentBase[] allComponents = FindObjectsOfType<ComponentBase>();

            if (allComponents.Any())
            {
                // todo: Eventually once we have multiple possible targets, choose a random target from this list.
                ComponentBase nextTarget = allComponents.FirstOrDefault(ft => ft != null && ft is IFriendlyTarget);

                if (nextTarget != null)
                {
                    return nextTarget?.gameObject;
                }
            }
            return null;
        }

        private void Die()
        {
            if (explosionPrefab != null)
            {
                InstantiateInLevel(explosionPrefab, transform.position);
            }

            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            Destroy(gameObject);
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
            return Mathf.Clamp(expDistance, 0, moveSpeed);
        }

        public override void ProjectileCollided(Collision2D collision)
        {
            Die();
            base.ProjectileCollided(collision);
        }

    }
}
