using Assets.Source.Components.Actor;
using Assets.Source.Components.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class HealthBarComponent : HUDBase
    {
        private readonly float LERP_THRESHOLD = .025f;

        private Animator Animator;
        private ActorBehavior ActorBehavior;
        private Image healthImage;
        private int presentedHealth;

        public override void ComponentStart()
        {
            Animator = GetRequiredComponent<Animator>();
            healthImage = GetRequiredComponent<Image>();
            ActorBehavior = GetPlayerActorBehavior();

            presentedHealth = ActorBehavior.Health;

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if(ActorBehavior.Health != presentedHealth)
            {
                RunHealthAnimation();
            }
            else
            {
                StopHealthAnimation();
            }
            base.ComponentUpdate();
        }

        public void StopHealthAnimation()
        {
            Animator.SetBool("highlight", false);
        }

        public void RunHealthAnimation()
        {
            Animator.SetBool("highlight", true);

            float startingPercent = (float)presentedHealth / ActorBehavior.MaxHealth;
            float endingPercent = (ActorBehavior.Health <= 0) ? 0.0f : (float)ActorBehavior.Health / ActorBehavior.MaxHealth;
            float percentReductionLerped = Mathf.Lerp(startingPercent, endingPercent, 0.01f);
            int lerpedHealth = (int)(percentReductionLerped * 100);

            //because lerping will never reach its target we need to have some close threshold
            //once reached we set the visual to be exactly right
            if (percentReductionLerped - LERP_THRESHOLD < ActorBehavior.Health)
            {
                presentedHealth = ActorBehavior.Health;
                healthImage.fillAmount = lerpedHealth == 0 ? 0 : (float) presentedHealth / ActorBehavior.MaxHealth;
            }
            else
            {
                presentedHealth = lerpedHealth;
                healthImage.fillAmount = percentReductionLerped;
            }
        }
    }
}