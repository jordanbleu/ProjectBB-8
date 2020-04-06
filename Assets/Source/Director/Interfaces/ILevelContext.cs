namespace Assets.Source.Director.Interfaces
{
    public interface ILevelContext
    {
        /// <summary>
        /// This is the currently active phase
        /// </summary>
        ILevelPhase CurrentPhase { get; }
        
        /// <summary>
        /// If set to true, the phase is completed and ready to move on.  The Director
        /// will see this and know to call <seealso cref="CompletePhase"/> when its ready
        /// </summary>
        bool IsCompleted { get; }
        
        /// <summary>
        /// Calls the current phase's update method
        /// </summary>
        void UpdatePhase();

        /// <summary>
        /// Calls the current phase's complete method
        /// </summary>
        void CompletePhase();

        /// <summary>
        /// Begins a new phase of a level.  This will call PhaseBegin on the current level phase,
        /// and kick off the phase updates.
        /// </summary>
        void BeginPhase(ILevelPhase phase);

        /// <summary>
        /// Signals to the Director that the phase is completed, and <seealso cref="CompletePhase"/>
        /// should be called when ready
        /// </summary>
        void FlagAsComplete();

    }
}
