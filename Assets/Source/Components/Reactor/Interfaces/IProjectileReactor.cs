using UnityEngine;

namespace Assets.Source.Components.Reactor.Interfaces
{
    public interface IProjectileReactor
    {
        void ReactToProjectileHit(Collision2D collision, int baseDamage);
    }
}
