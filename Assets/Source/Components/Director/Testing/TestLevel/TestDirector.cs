using Assets.Source.Components.Director.Base;
using Assets.Source.Components.Director.Interfaces;

namespace Assets.Source.Components.Director.Testing.TestLevel
{
    public class TestDirector : DirectorComponentBase
    {
        // Generally for the context we'll do a lazy / caching implementation like this
        private ILevelContext _context;
        protected override ILevelContext Context 
        {
            get 
            {
                if (_context == null)
                {
                    _context = new LevelContext();
                }
                return _context;
            } 
        }
        

        protected override void SetStartPhase()
        {
            Context.BeginPhase<TestLevelPhase1>();
        }
    }
}
