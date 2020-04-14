using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Player;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.UI.Base
{
    /// <summary>
    /// The base class that all HUD elements should use if they need to update based on player data.
    /// This class provides consistent access to the player and helper methods.
    /// </summary>
    public class HUDBase : ComponentBase
    {
        protected GameObject player;

        public override void ComponentAwake()
        {
            player = GetRequiredObject(GameObjects.Actors.Player);

            base.ComponentAwake();
        }

        public PlayerBehavior GetPlayerBehavior()
        {
            return GetRequiredComponent<PlayerBehavior>(player);
        }

        public ActorBehavior GetPlayerActorBehavior()
        {
            return GetRequiredComponent<ActorBehavior>(player);
        }
    }
}