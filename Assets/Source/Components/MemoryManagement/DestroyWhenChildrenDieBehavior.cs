using Assets.Source.Components.Base;

namespace Assets.Source.Components.MemoryManagement
{
    /// <summary>
    /// This highly complex component will destroy the attached object if all of his children are destroyed
    /// </summary>
    public class DestroyWhenChildrenDieBehavior : ComponentBase
    {
        public override void ComponentUpdate()
        {
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
            base.ComponentUpdate();
        }


    }
}
