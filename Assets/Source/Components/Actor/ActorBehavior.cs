using Assets.Source.Components.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        // todo: make this not show in the inspector eventually
        [SerializeField]
        private int _health = 100;
        public int Health 
        {            
            get => _health; 
            set => _health = value; 
        }

        [SerializeField]
        private int _blasterAmmo = 50;
        public int BlasterAmmo 
        {
            get => _blasterAmmo; 
            set => _blasterAmmo = value; 
        }

        #region Player Stats
        public int MaxHealth { get; set; } = 100;

        public int MaxBlasterAmmo { get; set; } = 150;
        #endregion
    }
}
