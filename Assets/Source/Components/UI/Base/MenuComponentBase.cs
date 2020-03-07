using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.UI.Base
{
    public abstract class MenuComponentBase : ComponentBase
    {
        [SerializeField]
        private GameObject _firstSelectedItem;
        /// <summary>
        /// The button in the UI that should be selected
        /// </summary>
        public GameObject FirstSelectedItem => _firstSelectedItem;

        public override void PerformAwake()
        {
            if (FirstSelectedItem == null)
            {
                throw new UnityException($"Menu Component {this.gameObject.name} must specify a gameobject for 'First Selected Item'.  " +
                    $"Drag and drop the object via the inspector window.");
            }
            base.PerformAwake();
        }


    }
}
