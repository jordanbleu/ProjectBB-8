using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Animation
{
    /// <summary>
    /// This component can be used for simple objects with common animations.  This component
    /// requires a rigid body to exist on the object.
    /// 
    /// Criteria Include:
    /// <para>- Has a move left and a move right animation</para>
    /// <para>- Has an idle animation</para>
    /// </summary>
    public class AutoAnimatorComponent : ComponentBase
    {
        [SerializeField]
        private string horizontalSpeedAnimatorParameter = "horizontal_move";

        private Rigidbody2D rigidBody;
        private Animator animator;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {            
            int horizontalMovement = 0;

            // if the actor is pointing downwards, reverse its animation
            bool isRotated = transform.eulerAngles.z >= 180;

            if (rigidBody.velocity.x > 0) {
                horizontalMovement = (isRotated) ? -1 : 1;
            } else if (rigidBody.velocity.x < 0) {
                horizontalMovement = (isRotated) ? 1 : -1;
            }

            animator.SetInteger(horizontalSpeedAnimatorParameter, horizontalMovement);
            base.ComponentUpdate();
        }
    }
}
