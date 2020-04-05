using Assets.Source.Director.Interfaces;

namespace Assets.Source.Director
{
    public class LevelContext : ILevelContext
    {
        public ILevelPhase CurrentPhase { get; private set; }
        public bool IsCompleted { get; private set; } = false;

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
    }
}
