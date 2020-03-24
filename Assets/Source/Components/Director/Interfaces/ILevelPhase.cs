namespace Assets.Source.Components.Director.Interfaces
{
    /// <summary>
    /// A level phase represents a single unit / section of a level.  It has completion criteria 
    /// and the ability to swap to the next phase, or mark the level as completed
    /// </summary>
    public interface ILevelPhase
    {
        /// <summary>
        /// This is the initialization for the phase.  Things like spawning enemies, starting dialogue, and 
        /// others should happen here.  This method is only called once.
        /// </summary>
        /// <param name="context"></param>
        void PhaseBegin(ILevelContext context);

        /// <summary>
        /// While the phase is active, this is called once per frame.  Use this method to perform checks
        /// for phase complete criteria.  Generally used to check for the existence of objects spawned in <seealso cref="PhaseBegin(ILevelContext)"/>
        /// </summary>
        /// <param name="context"></param>
        void PhaseUpdate(ILevelContext context);

        /// <summary>
        /// This method gets called whenever the director decides it is okay with moving to the next phase.  
        /// Use this to set the next phase via <seealso cref="ILevelContext.BeginPhase{TPhase}"/>
        /// </summary>
        /// <param name="context"></param>
        void PhaseComplete(ILevelContext context);
    }
}
