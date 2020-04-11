using Assets.Source.Components.Base;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class SnackbarNotificationComponent : ComponentBase
    {
        private Animator animator;
        private AudioSource audioSource;

        private AudioClip warningNoise;

        public override void ComponentAwake()
        {
            animator = GetRequiredComponent<Animator>();
            audioSource = GetRequiredComponent<AudioSource>();

            warningNoise = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/UI/warning");
            base.ComponentAwake();
        }

        public override void ComponentOnEnable()
        {
            audioSource.PlayOneShot(warningNoise);
            animator.SetTrigger("show");
            base.ComponentOnEnable();
        }

        public void DeactivateSelf()
        {
            gameObject.SetActive(false);
        }


    }
}
