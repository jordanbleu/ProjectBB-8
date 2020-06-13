using Assets.Source.Components.Base;
using Assets.Source.Director.Interfaces;

namespace Assets.Source.Director.LevelPhases.Demo
{
    public class DP_100_IntroDialogue : ILevelPhase
    {
        public void PhaseBegin(ILevelContext context)
        {
            ComponentBase.InitiateDialogueExchange("Demo/introSequence.xml");
        }

        public void PhaseComplete(ILevelContext context)
        {

        }

        public void PhaseUpdate(ILevelContext context)
        {

        }
    }
}
