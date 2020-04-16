using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class HealthBarComponent : ComponentBase
    {
        private Animator animator;
        private ActorBehavior actorBehavior;
        private Image healthImage;
        private int presentedHealth;

        public override void ComponentStart()
        {
            animator = GetRequiredComponent<Animator>();
            healthImage = GetRequiredComponent<Image>();
            GameObject player = GetRequiredObject(GameObjects.Actors.Player);
            actorBehavior = GetRequiredComponent<ActorBehavior>(player);

            presentedHealth = actorBehavior.Health;

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if(actorBehavior.Health != presentedHealth)
            {
                animator.SetBool("highlight", true);

                if(actorBehavior.Health < presentedHealth)
                {
                    presentedHealth--;
                }
                else
                {
                    presentedHealth++;
                }
            }
            else
            {
                animator.SetBool("highlight", false);
            }

            healthImage.fillAmount = (float)presentedHealth / actorBehavior.MaxHealth;

            base.ComponentUpdate();
        }
    }
}