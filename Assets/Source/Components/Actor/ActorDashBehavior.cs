using Assets.Source.Components.Base;
using Assets.Source.Components.Timer;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Actor
{
    public class ActorDashBehavior : ComponentBase
    {
        /// <summary>
        /// Returns true if the actor has the dash available
        /// </summary>
        public bool CanDash { get; private set; } = false;

        /// <summary>
        /// Returns true if the actor is currently dashing.
        /// Should be used in conjunction with <see cref="Dash(Vector2)"/> if this returns true
        /// </summary>
        public bool IsDashing { get; private set; } = false;

        public float DashDistance { get; set; } = 1.5f;
        public float CooldownTime { get; set; } = 2000.0f; //default to 2 seconds
        public int MaxDashesAvailable { get; set; } = 1;

        //External Components
        private IntervalTimerComponent dashTimer;
        private ActorRestrictorComponent actorRestrictor;
        private ParticleSystem dashParticles;

        private int dashesAvailable;
        private float dashEndLocation;
        private enum DashDirections
        {
            Left,
            Right
        }
        private DashDirections dashDirection;

        public override void ComponentAwake()
        {
            SetupDashTimer();
            actorRestrictor = GetRequiredComponent<ActorRestrictorComponent>();

            GameObject dashEffect = GetRequiredChild("DashEffect");
            dashParticles = GetRequiredComponent<ParticleSystem>(dashEffect);

            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            dashesAvailable = MaxDashesAvailable;
            base.ComponentStart();
        }

        /// <summary>
        /// Because the ActorDash can't assign external velocity the actor needs to call this method when trying to dash.
        /// This determines if a Dash if available and if it is then the dash component is setup and ready.
        /// </summary>
        /// <returns>True if the actor has a dash available and is not currently dashing</returns>
        public bool TrySetupDashLeft()
        {
            if (CanDash && !IsDashing)
            {
                dashEndLocation = transform.position.x - DashDistance;
                IsDashing = !IsDashing;
                dashDirection = DashDirections.Left;
            }

            return CanDash;
        }

        /// <summary>
        /// Because the ActorDash can't assign external velocity the actor needs to call this method when trying to dash.
        /// This determines if a Dash if available and if it is then the dash component is setup and ready.
        /// </summary>
        /// <returns>True if the actor has a dash available and is not currently dashing</returns>
        public bool TrySetupDashRight()
        {
            if (CanDash && !IsDashing)
            {
                dashEndLocation = transform.position.x + DashDistance;
                IsDashing = !IsDashing;
                dashDirection = DashDirections.Right;
            }

            return CanDash;
        }

        /// <summary>
        /// Call for all frames where the actor should be dashing by checking the value of <see cref="IsDashing"/>
        /// </summary>
        /// <param name="externalVelocity">The current external velocity of the actor</param>
        /// <returns>The calculated velocity that should be assigned to the actor</returns>
        public Vector2 Dash(Vector2 externalVelocity)
        {
            if (CanDash)
            {
                if (dashDirection == DashDirections.Left)
                {
                    return DashLeft(externalVelocity);
                }
                else
                {
                    return DashRight(externalVelocity);
                }
            }

            return new Vector2(0, 0);
        }

        public float GetTimerCurrentTime()
        {
            return dashTimer.CurrentTime;
        }

        public float GetInterval()
        {
            return dashTimer.GetInterval();
        }

        public bool IsActive()
        {
            return dashTimer.IsActive;
        }

        private Vector2 DashLeft(Vector2 externalVelocity)
        {
            if (transform.position.x < dashEndLocation)
            {
                return EndDash(externalVelocity);
            }
            else
            {
                float xVel = -DashDistance + externalVelocity.x;
                if (actorRestrictor.DidHitBorder(xVel))
                {
                    return EndDash(externalVelocity);
                }

                ParticleSystem.ShapeModule shapeModule = dashParticles.shape;
                shapeModule.rotation = shapeModule.rotation.Copy(y: 90.0f);
                dashParticles.Play();

                return externalVelocity.Copy(x: xVel);
            }
        }

        private Vector2 DashRight(Vector2 externalVelocity)
        {
            if (transform.position.x > dashEndLocation)
            {
                return EndDash(externalVelocity);
            }
            else
            {
                float xVel = DashDistance + externalVelocity.x;
                if (actorRestrictor.DidHitBorder(xVel))
                {
                    return EndDash(externalVelocity);
                }

                ParticleSystem.ShapeModule shapeModule = dashParticles.shape;
                shapeModule.rotation = shapeModule.rotation.Copy(y: 270.0f);
                dashParticles.Play();

                return externalVelocity.Copy(x: xVel);
            }
        }

        private Vector2 EndDash(Vector2 externalVelocity)
        {
            IsDashing = !IsDashing;
            if(--dashesAvailable == 0){
                CanDash = !CanDash;
                dashesAvailable = MaxDashesAvailable;
                dashTimer.Reset();
            }

            return externalVelocity.Copy(x: 0);
        }

        private void SetupDashTimer()
        {
            dashTimer = gameObject.AddComponent<IntervalTimerComponent>();
            dashTimer.UpdateInterval(CooldownTime);
            dashTimer.AutoReset = false;
            dashTimer.OnIntervalReached.AddListener(OnIntervalReached);
        }

        private void OnIntervalReached()
        {
            CanDash = true;
        }
    }
}