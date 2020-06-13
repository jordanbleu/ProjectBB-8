using Assets.Source.Components.Base;
using Assets.Source.Components.Director.Base;
using Assets.Source.Director.Interfaces;
using UnityEngine;

namespace Assets.Source.Director
{
    public class LevelContext : ILevelContext
    {
        public ILevelPhase CurrentPhase { get; private set; }
        public bool IsCompleted { get; private set; } = false;

        private DirectorComponent director;
        public LevelContext(DirectorComponent directorComponent) {
            director = directorComponent;        
        }

        public void CompletePhase()
        {
            CurrentPhase.PhaseComplete(this);
        }

        public void UpdatePhase()
        {
            CurrentPhase.PhaseUpdate(this);
        }

        public void FlagAsComplete()
        {
            IsCompleted = true;
        }

        public void BeginPhase(ILevelPhase phase)
        {
            CurrentPhase = phase;
            IsCompleted = false;
            CurrentPhase.PhaseBegin(this);
        }

        public GameObject FindCanvas() => director.FindCanvasObject();
    }
}
