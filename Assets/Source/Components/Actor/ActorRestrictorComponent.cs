using Assets.Source.Components.Base;
using Assets.Source.Extensions;
using Assets.Source.Mathematics;
using System;
using UnityEngine;

namespace Assets.Source.Components.Actor
{
    public class ActorRestrictorComponent : ComponentBase
    {
        // Square that defines the area that surrounds this object
        [SerializeField]
        [Header("The Blue Square")]
        private Square myArea = new Square(0.5f,0.5f);

        // Square that defines the area to restrict this actor to
        [SerializeField]
        [Header("The Red Square")]
        private Square restrictedArea = new Square(4, 4);

        private float precision = 0.25f;
        private Rigidbody2D rigidBody;

        public override void ComponentAwake()
        {
            rigidBody = GetRequiredComponent<Rigidbody2D>();
            base.ComponentAwake();
        }

        private void LateUpdate()
        {
            float left = transform.position.x - myArea.Width / 2; //dividing by 2 makes this more accurate since position minus width is not left side
            float right = transform.position.x + myArea.Width / 2;
            float top = transform.position.y + myArea.Height / 2;
            float bottom = transform.position.y - myArea.Height / 2;

            if (left + (precision * Math.Sign(rigidBody.velocity.x)) < -restrictedArea.Width)
            {
                rigidBody.velocity = rigidBody.velocity.Copy(x: 0);
            }
            else if (right + (precision * Math.Sign(rigidBody.velocity.x)) > restrictedArea.Width)
            {
                rigidBody.velocity = rigidBody.velocity.Copy(x: 0);
            }

            if (top + (precision * Math.Sign(rigidBody.velocity.y)) > restrictedArea.Height)
            {
                rigidBody.velocity = rigidBody.velocity.Copy(y: 0);
            }
            else if (bottom + (precision * Math.Sign(rigidBody.velocity.y)) < -restrictedArea.Height)
            {
                rigidBody.velocity = rigidBody.velocity.Copy(y: 0);
            }
        }

        /// <summary>
        /// Calculates if the next frame the actor will be out of based based on the given velocity rather than the current vel
        /// </summary>
        /// <param name="xVel">The predicted velocity</param>
        /// <returns>True if the predicted location of the trasform will hit out of bounds</returns>
        public bool DidHitBorder(float xVel)
        {
            float left = transform.position.x - myArea.Width / 2;
            float right = transform.position.x + myArea.Width / 2;

            if (left + (precision * Math.Sign(xVel)) < -restrictedArea.Width)
            {
                return true;
            }
            else if (right + (precision * Math.Sign(xVel)) > restrictedArea.Width)
            {
                return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(restrictedArea.TopLeft, restrictedArea.TopRight);
            Gizmos.DrawLine(restrictedArea.TopRight, restrictedArea.BottomRight);
            Gizmos.DrawLine(restrictedArea.BottomRight, restrictedArea.BottomLeft);
            Gizmos.DrawLine(restrictedArea.BottomLeft, restrictedArea.TopLeft);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + (Vector3)myArea.TopLeft,    transform.position + (Vector3)myArea.TopRight);
            Gizmos.DrawLine(transform.position + (Vector3)myArea.TopRight,   transform.position + (Vector3)myArea.BottomRight);
            Gizmos.DrawLine(transform.position + (Vector3)myArea.BottomRight,transform.position + (Vector3)myArea.BottomLeft);
            Gizmos.DrawLine(transform.position + (Vector3)myArea.BottomLeft, transform.position + (Vector3)myArea.TopLeft);
        }
    }
}
