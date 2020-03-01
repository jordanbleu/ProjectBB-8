using Assets.Source.Components.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Components.Timer
{
    /// <summary>
    /// The interval timer component invokes the attached method delegate repeatedly on an interval.
    /// This interval is independent of the framerate of your game.
    /// </summary>
    public class IntervalTimerComponent : ComponentBase
    {
  
        // The time in milliseconds the interval timer counts down 
        // before invoking the unity event 
        [SerializeField]
        private float interval = 500f;

        public UnityEvent OnIntervalReached;

        private float currentTime = 0.0f;

        public override void Step()
        {
            currentTime += Time.deltaTime * 1000;

            if (currentTime >= interval)
            {
                currentTime = 0.0f;
                OnIntervalReached?.Invoke();
            }
            
            base.Step();
        }

        public void UpdateInterval(float interval)
        {
            currentTime = 0.0f;
            this.interval = interval;
        }

    }
}