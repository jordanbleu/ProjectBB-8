using Assets.Source.Components.LevelDirector.Base;

namespace Assets.Source.Components.Testing
{
    public class EmptyLevelBehavior : LevelDirectorComponentBase
    {

        public override void ComponentStart()
        {
            InitiateDialogueExchange("Testing/test.xml");        
        }


    }
}
