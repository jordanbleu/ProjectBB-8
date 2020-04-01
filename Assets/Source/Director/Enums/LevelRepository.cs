﻿using Assets.Source.Components.Director.Testing.TestLevel;
using Assets.Source.Director.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Director.Enums
{
    public static class LevelRepository
    {
        // todo: Eventually, these will also be the levels that get saved.
        /// <summary>
        /// Levels should be defined here.  
        /// 
        /// </summary>
        public enum Level
        {
            TestLevel = 0,
            Level_01  = 1,
            Level_02  = 2,
        }

        public static ILevelPhase FindStartPhase(Level level) 
        {
            // use a nasty switch case so that level phases are only instantiated 
            // if and when they are needed
            switch (level)
            {
                case Level.TestLevel:
                    return new TestLevelPhase1() as ILevelPhase;
                case Level.Level_01:
                case Level.Level_02:
                default:
                    throw new ArgumentException($"The specified level {level.ToString()} does not return a " +
                        $"start phase in the LevelRespository.FindStartPhase() method.", nameof(level));
            }
        }
    }
}