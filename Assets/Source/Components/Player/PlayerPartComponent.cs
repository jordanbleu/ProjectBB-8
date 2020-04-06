using Assets.Source.Components.Base;
using Assets.Source.Components.Projectile.Base;
using Assets.Source.Components.Reactor.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Player
{
    public class PlayerPartComponent : ComponentBase, IProjectileReactor
    {
        [SerializeField]
        private float partDamageMultiplier = 1;

        private PlayerBehavior playerBehavior;

        public override void ComponentAwake()
        {
            playerBehavior = GetRequiredComponentInParent<PlayerBehavior>();
            base.ComponentAwake();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            playerBehavior.ReactToHit(collision, baseDamage, partDamageMultiplier);
        }
    }
}
