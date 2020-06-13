using Assets.Source.Components.Base;
using Assets.Source.Constants;
using Assets.Source.Director.Interfaces;
using UnityEngine;

namespace Assets.Source.Director.LevelPhases.Demo
{
    public class DP_000_IntroFadeIn : ILevelPhase
    {
        private GameObject fadeObject;

        public void PhaseBegin(ILevelContext context)
        {
            GameObject basicFadeInPrefab = ComponentBase.GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Cameras/CameraEffects/BasicFadeIn");
            fadeObject = ComponentBase.InstantiatePrefab(basicFadeInPrefab, context.FindCanvas().transform);
        }

        public void PhaseUpdate(ILevelContext context)
        {
            if (fadeObject == null) 
            {
                context.FlagAsComplete();
            }
            
        }


        public void PhaseComplete(ILevelContext context)
        {
            context.BeginPhase(new DP_100_IntroDialogue());
        }


    }
}
