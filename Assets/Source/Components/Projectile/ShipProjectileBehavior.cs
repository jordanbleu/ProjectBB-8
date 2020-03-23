using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Projectile
{
    /// <summary>
    /// Every type of ship is technically a projectile so they should react when they hit each other. Add this script to any ship prefab
    /// </summary>
    public class ShipProjectileBehavior : ProjectileComponentBase, IProjectileReactor
    {
        protected override int BaseDamage => 50;

        public override void ComponentStart()
        {
            base.ComponentStart();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage) { }
    }
}
