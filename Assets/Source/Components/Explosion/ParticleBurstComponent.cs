using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Explosion
{
    public class ParticleBurstComponent : ComponentBase
    {
        private ParticleSystem particlesSystemComponent;

        public override void ComponentAwake()
        {
            particlesSystemComponent = GetRequiredComponent<ParticleSystem>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (!particlesSystemComponent.IsAlive())
            {
                Destroy(gameObject);
            }
            base.ComponentUpdate();
        }

    }
}
