using Assets.Source.Components.Base;
using Assets.Source.Extensions;
using Assets.Source.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Actor
{
    public class ActorRestrictorComponent : ComponentBase
    {

        // Square the defines the area that surrounds this object
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
            float left = transform.position.x - myArea.Width;
            float right = transform.position.x + myArea.Width;
            float top = transform.position.y + myArea.Height;
            float bottom = transform.position.y - myArea.Height;

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
