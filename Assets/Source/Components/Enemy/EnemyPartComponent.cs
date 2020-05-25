using Assets.Source.Components.AI.Interfaces;
using Assets.Source.Components.Base;
using Assets.Source.Components.Reactor.Interfaces;
using Assets.Source.Constants;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Enemy
{
    public class EnemyPartComponent : ComponentBase, IProjectileReactor
    {

        [Tooltip("Increase / Decrease damage done to this part.  Numbers greater than 1 will increase" +
            "base damage, numbers less than 1 will decrease it.")]
        [SerializeField]
        private float damageMultiplier = 1;

        private IEnemyPartContainer enemy;

        public override void ComponentAwake()
        {
            // Scan the parent components for an implementation of IEnemyPartContainer
            enemy = GetComponentsInParent<ComponentBase>().FirstOrDefault(comp => comp is IEnemyPartContainer) as IEnemyPartContainer
                ?? throw new UnityException($"Parent must contain a component that implements {nameof(IEnemyPartContainer)}");
            base.ComponentAwake();
        }

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            if (!collision.otherCollider.name.Equals(GameObjects.Projectiles.EnemyBullet))
            {
                enemy.OnEnemyPartHitWithProjectile(collision, (int)(baseDamage*damageMultiplier));
            }
        }
    }
}
