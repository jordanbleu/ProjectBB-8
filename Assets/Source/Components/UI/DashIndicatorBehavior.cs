using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class DashIndicatorBehavior : ComponentBase
    {
        private ActorDashBehavior actorDashBehavior;
        private Image cooldownImage;

        public override void ComponentStart()
        {
            GameObject player = GetRequiredObject(GameObjects.Actors.Player);
            actorDashBehavior = GetRequiredComponent<ActorDashBehavior>(player);
            cooldownImage = GetRequiredComponent<Image>();

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if (actorDashBehavior.IsActive())
            {
                float dashCooldownTime = actorDashBehavior.GetTimerCurrentTime();
                cooldownImage.fillAmount = (dashCooldownTime == 0.0f)
                    ? 0.0f
                    : 1 - dashCooldownTime / actorDashBehavior.CooldownTime;
            }
            
            base.ComponentUpdate();
        }
    }
}