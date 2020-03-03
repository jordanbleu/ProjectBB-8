using Assets.Source.Components.Base;
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
    public class ActorBehavior : ComponentBase
    {
        [SerializeField]
        private int _health;
        public int Health 
        {            
            get => _health; 
            set => _health = value; 
        }
    }
}
