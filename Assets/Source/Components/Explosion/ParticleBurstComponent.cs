using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Explosion
{
    public class ParticleBurstComponent : ComponentBase
    {
        private ParticleSystem particlesSystemComponent;

        public override void PerformAwake()
        {
            particlesSystemComponent = GetRequiredComponent<ParticleSystem>();
            base.PerformAwake();
        }

        public override void PerformUpdate()
        {
            if (!particlesSystemComponent.IsAlive())
            {
                Destroy(gameObject);
            }
            base.PerformUpdate();
        }

    }
}
