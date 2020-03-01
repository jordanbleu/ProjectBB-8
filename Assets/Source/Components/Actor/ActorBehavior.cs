using Assets.Source.Components.Base;
using Assets.Source.Components.Reactor.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Actor
{
    /// <summary>
    /// Any object with an attached ActorBehavior can interact with the game world naturally, 
    /// and can be affected by things in game.  They also maintain a status with things such as health, etc
    /// <para>
    /// This component doesn't do anything on its own, and so another behavior on this object should be 
    /// responsible for checking the status 
    /// </para>
    /// </summary>
    public class ActorBehavior : ComponentBase, IProjectileReactor
    {
        [SerializeField]
        private int _health;
        public int Health => _health;

        public void ReactToProjectileHit(Collision2D collision, int baseDamage)
        {
            _health -= baseDamage;
        }

        public override void Step()
        {
            if (_health <= 0) {
                Debug.LogError("YOU LOST THE GAME!?!?!?!");
            }
            base.Step();
        }
    }
}
