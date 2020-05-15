using Assets.Source.Director.Interfaces;

namespace Assets.Source.Director.LevelPhases.Sandbox
{
    public class EmptyLevelPhase : ILevelPhase
    {
        public void PhaseBegin(ILevelContext context)
        {
            // Nothing
            
            // Uncomment if testing dialogue
            //ComponentBase.InitiateDialogueExchange("Testing/test.xml");

        }

        public void PhaseComplete(ILevelContext context)
        {
            // Nada
        }

        public void PhaseUpdate(ILevelContext context)
        {
            // Nope
        }
    }
}
