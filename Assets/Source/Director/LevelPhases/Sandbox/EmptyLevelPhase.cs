﻿using Assets.Source.Director.Interfaces;
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