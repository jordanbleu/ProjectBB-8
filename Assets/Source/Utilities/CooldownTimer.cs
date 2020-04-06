using System;
using System.Collections;
using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Utilities
{
    public class CooldownTimer : ComponentBase
    {
        public float CooldownTime { get; set; } = 1.0f; //default this for now in case we don't set it
        /// <summary>
        /// If true this timer will continuously run and call its event every <seealso cref="CooldownTime"/> amount of Time.delta time.
        /// </summary>
        public bool AutoRun { get; set; } = false;

        private float timeRemaining;
        public bool Running { get; private set; } = false;

        public override void ComponentStart()
        {
            timeRemaining = CooldownTime;
        }

        public override void ComponentUpdate()
        {
            if (Running)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0.0f)
                {
                    OnCooldownFinished(EventArgs.Empty);

                    if (AutoRun)
                    {
                        Restart();
                    }
                    else
                    {
                        Complete();
                    }
                }
            }
            
            base.ComponentUpdate();
        }

        public void Start()
        {
            Running = true;
        }

        public void Resume()
        {
            Running = true;
        }

        public void Pause()
        {
            Running = false;
        }

        public void Restart()
        {
            Reset();
            Resume();
        }

        public void Complete()
        {
            Reset();
            Pause();
        }

        /// <summary>
        /// Resets the cooldown, should only be called after the subsuquent action performed required a cooldown
        /// </summary>
        public void Reset()
        {
            timeRemaining = CooldownTime;
        }

        /// <summary>
        /// The event that fires when the cooldown is finished. This should be used if additional logic is needed such as a sound or animation
        /// </summary>
        public event EventHandler CooldownFinished;

        private void OnCooldownFinished(EventArgs e)
        {
            CooldownFinished?.Invoke(this, e);
        }
    }
}