using UnityEngine;

namespace Assets.Source.Components.AI.Interfaces
{
    public interface IEnemyPartContainer
    {
        void OnEnemyPartHitWithProjectile(Collision2D collision, int baseDamage);
    }
}
