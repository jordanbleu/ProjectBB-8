using Assets.Source.Components.Base;
using Assets.Source.Components.Director.Base;

namespace Assets.Source.Components.Actor
{
    /// <summary>
    /// Attach this component to anything to automatically destroy it if it is outside the 
    /// director's boundaries
    /// </summary>
    public class DestroyObjectIfOffscreenBehavior : ComponentBase
    {
        private DirectorComponent director;

        public override void ComponentAwake()
        {
            director = GetRequiredComponent<DirectorComponent>(FindLevelObject());
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (transform.position.y > director.Boundaries.Height)
            {
                Destroy(gameObject);
            }
            else if (transform.position.y < -director.Boundaries.Height)
            {
                Destroy(gameObject);
            }

            if (transform.position.x > director.Boundaries.Width)
            {
                Destroy(gameObject);
            }
            else if (transform.position.x < -director.Boundaries.Width)
            {
                Destroy(gameObject);
            }

            base.ComponentUpdate();
        }
    }
}
