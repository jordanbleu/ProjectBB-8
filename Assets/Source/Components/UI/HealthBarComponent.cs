using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class HealthBarComponent : ComponentBase
    {
        public Animator Animator { get; private set; }

        public override void ComponentAwake()
        {
            Animator = GetRequiredComponent<Animator>();

            base.ComponentAwake();
        }

        public void StopHighlight()
        {
            Animator.SetBool("Highlight", false);
        }
    }
}


