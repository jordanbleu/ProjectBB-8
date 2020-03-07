using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.Camera
{
    public class CameraEffectComponent : ComponentBase
    {
        
        [SerializeField]
        private CameraModes _cameraMode = CameraModes.Handheld_1;
        public CameraModes CameraMode
        {
            get => _cameraMode; 
            set => _cameraMode = value; 
        }

        public enum CameraModes
        { 
            // A standard, mild sway effect that gives the feeling of flying
            Handheld_1 = 0,
            // A slightly more extreme variant that wobbles more
            Handheld_2 = 1
        }

        private Animator animator;

        public override void PerformAwake()
        {
            animator = GetRequiredComponent<Animator>();
            base.PerformAwake();
        }

        public override void PerformUpdate()
        {
            animator.SetFloat("camera_effect", (float)_cameraMode);
            base.PerformUpdate();
        }

        // todo: not sure if this is the best way to do this but its probably fine
        public void TriggerImpulse1()
        {
            animator.SetTrigger("trigger_impulse_1");        
        }


    }
}
