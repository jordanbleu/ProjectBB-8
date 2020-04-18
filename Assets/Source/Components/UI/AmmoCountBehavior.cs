using Assets.Source.Components.Actor;
using Assets.Source.Components.Base;
using Assets.Source.Components.Player;
using Assets.Source.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class AmmoCountBehavior : ComponentBase
    {
        private ActorBehavior actorBehavior;
        private PlayerBehavior playerBehavior;
        private Image cooldownImage;
        private TextMeshProUGUI ammoCountText;
        private int presentedAmmoCount;

        public override void ComponentStart()
        {
            GameObject cooldownIndicator = GetRequiredChild("CooldownIndicator");
            cooldownImage = GetRequiredComponent<Image>(cooldownIndicator);

            GameObject ammoCount = GetRequiredChild("AmmoCount");
            ammoCountText = GetRequiredComponent<TextMeshProUGUI>(ammoCount);

            GameObject player = GetRequiredObject(GameObjects.Actors.Player);
            actorBehavior = GetRequiredComponent<ActorBehavior>(player);
            playerBehavior = GetRequiredComponent<PlayerBehavior>(player);

            ammoCountText.SetText($"x{actorBehavior.BlasterAmmo}");
            presentedAmmoCount = actorBehavior.BlasterAmmo;

            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {
            if (presentedAmmoCount != actorBehavior.BlasterAmmo)
            {
                ammoCountText.SetText($"x{actorBehavior.BlasterAmmo}");
                presentedAmmoCount = actorBehavior.BlasterAmmo;
            }

            if (playerBehavior.ShootTimer.IsActive)
            {
                float currentCooldownTime = playerBehavior.ShootTimer.CurrentTime;
                cooldownImage.fillAmount = (currentCooldownTime == 0.0f)
                    ? 0.0f
                    : 1 - currentCooldownTime / playerBehavior.ShootTimer.GetInterval();
            }

            base.ComponentUpdate();
        }
    }
}