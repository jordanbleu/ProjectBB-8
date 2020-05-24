using Assets.Source.Components.Actor;
using Assets.Source.Components.AI.Interfaces;
using Assets.Source.Components.Base;
using Assets.Source.Components.Timer;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using Assets.Source.Mathematics;
using System;
using System.Linq;
using UnityEngine;


namespace Assets.Source.Components.AI
{
    [RequireComponent(typeof(AudioSource),typeof(ActorBehavior),typeof(Rigidbody2D))]
    public class ShooterEnemyAIBehavior : ComponentBase, IEnemyPartContainer
    {
        // Inspector Values
        [Header("Behavior Setup")]
        [Tooltip("The maximum duration between shots at the player.")]
        [SerializeField]
        private int shotFrequency = 500;

        [Tooltip("Maximum movement speed")]
        [SerializeField]
        private float moveSpeed = 2f;

        [Tooltip("How heavy the enemy is.  Larger numbers will make the enemy fly less far away when shot.")]
        [SerializeField]
        private float weight = 10;

        [Tooltip("Sets the ideal distance to maintain from the target (vertically).")]
        [SerializeField]
        private float distanceFromTarget = 1.5f;

        [Tooltip("The position padding represents the acceptable area surrounding the " +
            "target destination and the ideal destination.  Smaller numbers will make the " +
            "enemy move more eagerly towards a more precise position, based upon the 'distanceFromTarget' value" +
            "above.  Larger numbers will make the enemy remain stationary more often.  ")]
        [SerializeField]
        private Vector2 positionPadding = new Vector2(0.5f, 0.5f);

        [Tooltip("The amount of damage the enemy takes in releation to base damage.  Values under 1 " +
            "decrease damage taken, values over 1 increase it.")]
        [SerializeField]
        private float damageTakenMultiplier = 1;

        [Tooltip("The movement boundaries.  Generally should be similar or the same to the player's moveable area.")]
        [SerializeField]
        private Square movementArea = new Square(4,3);

        [Header("Object References")]
        [Tooltip("The bullet prefab to be spawned when shooting")]
        [SerializeField]
        private GameObject bulletPrefab;

        [Tooltip("The prefab to instantiate on death.  Can be left null. ")]
        [SerializeField]
        private GameObject explosionPrefab;

        [Header("Audio Clips")]
        [Tooltip("The enemy's blaster sound")]
        [SerializeField]
        private AudioClip blasterSound;

        [Tooltip("The sound to play on death.  Can be left null")]
        [SerializeField]
        private AudioClip deathSound;

        [Tooltip("The sound to play when hit by a projectile.  Can be left null.")]
        [SerializeField]
        private AudioClip projectileImpactSound;

        // Fields
        private GameObject currentTarget;
        private Vector2 idealPosition;
        private Vector2 destination; //todo: no public
        private Vector2 externalVelocity; //todo: no public

        // Consts
        private readonly float HORIZONTAL_SHOOT_RANGE = 0.5f;
        private readonly float MOVEMENT_THRESHOLD = 0.01f;
        private readonly float STABILIZATION_RATE = 0.05f;

        // Components
        private IntervalTimerComponent shootTimer;
        private AudioSource audioSource;
        private Rigidbody2D rigidBody;
        private ActorBehavior actorBehavior;

        public override void ComponentAwake()
        {
            CreateTimers();
            audioSource = GetRequiredComponent<AudioSource>();
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            actorBehavior = GetRequiredComponent<ActorBehavior>();

            externalVelocity = Vector2.zero;

            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            UpdateDestination();
            currentTarget = ChooseNextTarget();
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            // Re-randomize the random seed, factoring instance id for extra randomness per instance
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks - gameObject.GetInstanceID());

            if (actorBehavior.Health <= 0)
            {
                Die();
            }

            UpdatePositionValues();
            MoveTowardsPosition();
            StabilizeExternalVelocity();
            base.ComponentUpdate();
        }

        // Slowly stabilizes the external velocity
        private void StabilizeExternalVelocity()
        {
            float xv = externalVelocity.x.Normalize(STABILIZATION_RATE, 0);
            float yv = externalVelocity.y.Normalize(STABILIZATION_RATE, 0); 
            externalVelocity = new Vector2(xv, yv);
        }

        private void UpdatePositionValues()
        {
            if (currentTarget != null)
            {
                // The "ideal position" is the perfect position to shoot the target 
                Vector2 targetPos = currentTarget.transform.position;
                idealPosition = new Vector2(targetPos.x, targetPos.y + distanceFromTarget);

                // if the ideal position area moves outside of our destination area
                if (!idealPosition.IsWithin(positionPadding, destination))
                {
                    UpdateDestination();
                }
            }
        }

        // Select a new destination that is within the padding area of the ideal position
        private void UpdateDestination()
        {
            float newx = UnityEngine.Random.Range(idealPosition.x - positionPadding.x / 2,
                idealPosition.x + positionPadding.x / 2);

            float newy = UnityEngine.Random.Range(idealPosition.y - positionPadding.y / 2,
                idealPosition.y + positionPadding.y / 2);

            // Restrict the enemy to the game area
            newx = Mathf.Clamp(newx, -movementArea.Width, movementArea.Width);
            newy = Mathf.Clamp(newy, -movementArea.Height, movementArea.Height);

            // Choose a new destination that is within the position padding of the ideal position
            destination = new Vector2(newx, newy);
        }

        private void MoveTowardsPosition()
        {
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

            rigidBody.velocity = rigidBody.velocity.Copy(horizontalMoveDelta, verticalMoveDelta) + externalVelocity;
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



        // Initialize and attach all Interval Timer Components
        private void CreateTimers()
        {
            shootTimer = gameObject.AddComponent<IntervalTimerComponent>();
            shootTimer.SetInterval(shotFrequency);
            shootTimer.AutoReset = true;
            shootTimer.Randomize = true;
            shootTimer.OnIntervalReached.AddListener(ShotTimerReached);
            shootTimer.IsActive = true;
        }

        // Interval Component has decided we should shoot maybe.  
        private void ShotTimerReached() 
        {
            if (currentTarget != null)
            {
                bool playerInRange = currentTarget.transform.position.x.IsWithin(
                    HORIZONTAL_SHOOT_RANGE,
                    transform.position.x);

                if (playerInRange) { Shoot(); }
            }
        }

        private void Shoot()
        {
            audioSource.PlayOneShot(blasterSound);
            InstantiateInLevel(bulletPrefab, transform.position);
        }

        // Choose from the active IFriendlyTargets in the scene 
        private GameObject ChooseNextTarget() {

            ComponentBase[] allComponents = FindObjectsOfType<ComponentBase>();

            if (allComponents.Any())
            {
                // todo: Make this selection process more robust / interesting
                ComponentBase nextTarget = allComponents.FirstOrDefault(ft=>ft!=null && ft is IFriendlyTarget);

                if (nextTarget != null) {
                    return nextTarget?.gameObject;
                }                
            }    
            return null;
        }

        private void OnDrawGizmosSelected()
        {
            if (currentTarget != null) 
            {
                // The magenta square shows the position padding around the ideal position
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(idealPosition, positionPadding);

                // The magenta circle shows the ideal position (the center of the padding)
                Gizmos.DrawWireSphere(idealPosition, 0.01f);

                // The white line represents the enemy's destination
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, destination);

                // The white box shows the destination's position padding
                Gizmos.DrawWireCube(destination, positionPadding);

                // The red lines represent the enemy's shooting range
                Gizmos.color = Color.red;
                var left = new Vector2(transform.position.x - HORIZONTAL_SHOOT_RANGE, transform.position.y);
                var right = new Vector2(transform.position.x + HORIZONTAL_SHOOT_RANGE, transform.position.y);
                Gizmos.DrawLine(left, left.Copy(y: currentTarget.transform.position.y));
                Gizmos.DrawLine(right, right.Copy(y: currentTarget.transform.position.y));

                // The yellow lines represent the enemies boundaries
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(movementArea.Width*2, movementArea.Height*2, 0));
            }            
        }

        public void OnEnemyPartHitWithProjectile(Collision2D collision, int baseDamage)
        {
            Vector3 projectilePosition = collision.otherCollider.gameObject.transform.position;

            // Apply external velocity based on the impact point
            float impactSide = Mathf.Sign(transform.position.x - projectilePosition.x);

            float impactVelocity = (baseDamage / 10);

            externalVelocity = new Vector2(impactVelocity * impactSide, impactVelocity * 2);

            actorBehavior.Health -= (int)(baseDamage * damageTakenMultiplier);

            if (projectileImpactSound != null)
            {
                audioSource.PlayOneShot(projectileImpactSound);
            }
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
    }
}
