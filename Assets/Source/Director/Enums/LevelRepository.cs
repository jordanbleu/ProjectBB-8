using Assets.Source.Director.Interfaces;
using Assets.Source.Director.LevelPhases.Demo;
using Assets.Source.Director.LevelPhases.Sandbox;
using Assets.Source.Director.Testing.TestLevel;
using System;

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
            EmptySandBox  = 1,
            DemoLevel  = 2,
        }

        public static ILevelPhase FindStartPhase(Level level) 
        {
            // use a nasty switch case so that level phases are only instantiated 
            // if and when they are needed
            switch (level)
            {
                case Level.TestLevel:
                    return new TestLevelPhase1();
                case Level.EmptySandBox:
                    return new EmptyLevelPhase();
                case Level.DemoLevel:
                    return new DP_000_IntroFadeIn();
                default:
                    throw new ArgumentException($"The specified level {level.ToString()} does not return a " +
                        $"start phase in the LevelRespository.FindStartPhase() method.", nameof(level));
            }
        }
    }
}
