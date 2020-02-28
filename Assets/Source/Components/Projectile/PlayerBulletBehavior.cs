using Assets.Source.Components.Projectile.Base;
using Assets.Source.Extensions;

namespace Assets.Source.Components.Projectile
{
    public class PlayerBulletBehavior : ProjectileComponentBase
    {
        private readonly float MOVE_SPEED = 6f;

        public override void Create()
        {
            RigidBody.velocity = RigidBody.velocity.Copy(y: MOVE_SPEED);
            base.Construct();
        }

    }
}