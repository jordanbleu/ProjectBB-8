using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Explosion
{
    public class ParticleBurstComponent : ComponentBase
    {
        private ParticleSystem particlesSystemComponent;

        public override void Construct()
        {
            particlesSystemComponent = GetRequiredComponent<ParticleSystem>();
            base.Construct();
        }

        public override void Step()
        {
            if (!particlesSystemComponent.IsAlive())
            {
                Destroy(gameObject);
            }
            base.Step();
        }

    }
}
