using Assets.Source.Components.Base;
using Assets.Source.Components.Director.Interfaces;
using UnityEngine;

namespace Assets.Source.Components.Director.Testing.TestLevel
{
    public class TestLevelPhase3 : ILevelPhase
    {
        public void PhaseBegin(ILevelContext context)
        {
            ComponentBase.InitiateDialogueExchange("Testing/test.xml");
        }

        public void PhaseUpdate(ILevelContext context)
        {
            // The level director naturally won't advance phases
            // if there is an active text writer, so no need to 
            // check here <3
            context.FlagAsComplete();
        }

        public void PhaseComplete(ILevelContext context)
        {
            Debug.Log("You beat the entire game");
        }

    }
}
