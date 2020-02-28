using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Projectile.Base
{
    /// <summary>
    /// This class automatically handles memory management and other things for all projectiles
    /// </summary>
    public abstract class ProjectileComponentBase : ComponentBase
    { 
        protected Rigidbody2D RigidBody { get; private set; }

        public override void Construct()
        {
            RigidBody = GetRequiredComponent<Rigidbody2D>();
            base.Construct();
        }

        public override void Step()
        {
            // todo: these are just arbitrary numbers for now
            if (transform.position.y > 7) {
                Destroy(gameObject);
            }

            base.Step(); 
        }
    }
}
