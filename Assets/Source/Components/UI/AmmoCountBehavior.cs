using Assets.Source.Components.Actor;
using Assets.Source.Components.Player;
using Assets.Source.Components.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Components.UI
{
    public class AmmoCountBehavior : HUDBase
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

            actorBehavior = GetPlayerActorBehavior();
            playerBehavior = GetPlayerBehavior();

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