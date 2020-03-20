using Assets.Source.Components.Base;
using Assets.Source.Components.Reactor.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Player.Base
{
    public abstract class PlayerPartComponentBase : ComponentBase, IProjectileReactor
    {
        public abstract int BaseDamage { get; }



        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            throw new System.NotImplementedException();
        }
    }
}
