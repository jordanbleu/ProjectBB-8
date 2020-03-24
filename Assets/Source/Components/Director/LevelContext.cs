using Assets.Source.Components.Director.Interfaces;

namespace Assets.Source.Components.Director
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

        public void BeginPhase<TPhase>() where TPhase : ILevelPhase, new()
        {
            TPhase startPhase = new TPhase();
            CurrentPhase = startPhase;
            IsCompleted = false;
            CurrentPhase.PhaseBegin(this);
        }
    }
}
