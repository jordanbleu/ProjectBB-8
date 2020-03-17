using UnityEngine;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using System;

namespace Assets.Source.Components.Enemy.Base
{
    public abstract class EnemyAIBehavior : ComponentBase
    {
        private GameObject player;
        private float timeUntilNextShot;
        private float distanceAwayFromPlayer;

        public override void PerformAwake()
        {
            player = GetRequiredObject(GameObjects.Player);
            distanceAwayFromPlayer = transform.position.y - player.transform.position.y;
            timeUntilNextShot = TimeBetweenShots;
            base.PerformAwake();
        }

        /// <summary>
        /// The miminum unity units (left or right for now) that the enemy must be from the player before shooting
        /// </summary>
        public virtual float FireThreshold { get; } = .75f;

        /// <summary>
        /// Whether or not the enemy should try to remain the same distance away from the player
        /// </summary>
        public virtual bool ShouldKeepDistance { get; } = true;

        /// <summary>
        /// This is the value used to linearly interpolate towards the player. Must be a value between 0 (0%) and 1 (100%) interpolation.
        /// A value of 0 means the enemy will not move, a value of 1 will jump the enemy instantly to the given location.
        /// </summary>
        public virtual float EnemyMovementInterpolate { get; } = 0.05f;

        /// <summary>
        /// The Time.deltaTime between when the enemy can shoot while <seealso cref="ShouldFire(Transform)" retuns true./>
        /// </summary>
        public virtual float TimeBetweenShots { get; } = 1.5f;

        /// <summary>
        /// This automatically moves the enemy towards the player to try and attack.
        /// The base method only moves the enemy along the horizontal axis.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 UpdatedEnemyPosition(Transform transform)
        {
            Vector3 playerLocation = GetPlayerLocation();
            Vector3 lerpedLocation = Vector3.Lerp(transform.position, playerLocation, EnemyMovementInterpolate);
            //Debug.Log($"Lerped Location: {lerpedLocation}");

            float adjustedYPosition = 0f;
            if (ShouldKeepDistance)
            {
                float currentDistanceFromPlayer = 0f;
                if(player != null)
                {
                    currentDistanceFromPlayer = transform.position.y - player.transform.position.y;
                }
                
                adjustedYPosition = distanceAwayFromPlayer - currentDistanceFromPlayer;
            }
            
            Vector3 newEnemyPosition = new Vector3(lerpedLocation.x, transform.position.y + adjustedYPosition, transform.position.z);
            return newEnemyPosition;
        }

        /// <summary>
        /// Should be used within the Update method to determine whether or not the enemy should shoot.
        /// The base method only fires when close enough to the player.
        /// </summary>
        /// <param name="transform">The enemy transform</param>
        /// <returns></returns>
        public virtual bool ShouldFire(Transform transform)
        {
            bool shouldFire = false;
            Vector3 playerLocation = GetPlayerLocation();
            //fire if player is within horizontal range
            if(Math.Abs(playerLocation.x) < Math.Abs(transform.position.x) + FireThreshold)
            {
                timeUntilNextShot -= Time.deltaTime;
                if(timeUntilNextShot < 0)
                {
                    timeUntilNextShot = TimeBetweenShots;
                    shouldFire = true;
                }
            }

            return shouldFire;
        }

        private Vector3 GetPlayerLocation()
        {
            Vector3 playerLocation = new Vector3();
            if (player != null)
            {
                playerLocation = player.transform.position;
            }
            return playerLocation;
        }
    }
}