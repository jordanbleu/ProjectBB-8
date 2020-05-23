using Assets.Source.Components.Base;
using Assets.Source.Components.Pathing;
using Assets.Source.Constants;
using Assets.Source.Extensions;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    public class SeekerEnemy : ComponentBase
    {
        // Required Components
        private PathfinderComponent pathfinderComponent;
        private Rigidbody2D rigidBody;

        // Required GameObject References
        private GameObject playerObject;

        private readonly float ACCELERATION_RATE = 0.02f;
        private readonly float ACCELERATION_MAX = 2f;
        private readonly float PRECISION = 0.2f;

        public string debug = "poop";

        public override void ComponentAwake()
        {
            pathfinderComponent = GetRequiredComponent<PathfinderComponent>();
            rigidBody = GetRequiredComponent<Rigidbody2D>();

            playerObject = GetRequiredObject(GameObjects.Actors.Player);
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            pathfinderComponent.SeekPath(playerObject.transform.position);

            // Either seek to the next point in the path or just fly towards 
            // the player suicide bomber style
            Vector2 currentPoint = pathfinderComponent.CurrentPoint ?? playerObject.transform.position;

            float xv = rigidBody.velocity.x;
            float yv = rigidBody.velocity.y;

            if (transform.position.x.IsWithin(PRECISION, currentPoint.x)) {
                xv = 0;
            }
            if (transform.position.x > currentPoint.x)
            {
                xv -= ACCELERATION_RATE;
            }
            else if (transform.position.x < currentPoint.x)
            {
                xv += ACCELERATION_RATE;
            }

            if (transform.position.y.IsWithin(PRECISION, currentPoint.y))
            {
                yv = 0;
            }
            else if (transform.position.y > currentPoint.y)
            {
                yv -= ACCELERATION_RATE;
            }
            else if (transform.position.y < currentPoint.y) 
            {
                yv += ACCELERATION_RATE;
            }

            xv = Mathf.Clamp(xv, -ACCELERATION_MAX, ACCELERATION_MAX);
            yv = Mathf.Clamp(yv, -ACCELERATION_MAX, ACCELERATION_MAX);

            rigidBody.velocity = new Vector2(xv, yv);
            base.ComponentUpdate();
        }






    }
}
