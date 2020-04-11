using UnityEngine;

namespace Assets.Source.Components.Base
{
    /// <summary>
    /// This class works exactly the same way as the normal replicant behavior, but it 
    /// allows the ability to apply a dynamic delay in processing each frame.  To utilize this 
    /// feature, override the DelayedStep method instead of Step
    /// </summary>
    public abstract class DelayComponentBase : ComponentBase
    {
        /// <summary>
        /// Override this to set the delay in milliseconds
        /// </summary>
        protected int FrameTimeDelay { get; set; }

        private float elapsedFrameTime = 0;

        /// <summary>
        /// If you override this, BE SURE TO CALL THE BASE METHOD OR YOU'LL BREAK IT
        /// <para>
        /// NOTE: This will still run every frame, regardless of frame delay.  If you're trying 
        /// to use the frame delay stuff overrride <seealso cref="DelayedUpdate"/>
        /// </para>
        /// </summary>
        public override void ComponentUpdate()
        {
            elapsedFrameTime += (1000 * Time.deltaTime);

            if (elapsedFrameTime >= FrameTimeDelay)
            {
                DelayedUpdate();
                elapsedFrameTime = 0f;
            }
            base.ComponentUpdate();
        }

        /// <summary>
        /// Override this and pretend like it's the same as step but it will be delayed
        /// </summary>
        public abstract void DelayedUpdate();
    }
}
