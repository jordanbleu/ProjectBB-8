using Assets.Source.Components.Base;
using Assets.Source.Director.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Director.LevelPhases.Sandbox
{
    public class EmptyLevelPhase : ILevelPhase
    {
        public void PhaseBegin(ILevelContext context)
        {
            // Nothing
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
