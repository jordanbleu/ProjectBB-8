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

        protected CanvasMenuSelectorComponent menuSelector;

        public override void ComponentAwake()
        {
            menuSelector = GetRequiredComponent<CanvasMenuSelectorComponent>(FindOrCreateCanvas());

            if (FirstSelectedItem == null)
            {
                throw new UnityException($"Menu Component {this.gameObject.name} must specify a gameobject for 'First Selected Item'.  " +
                    $"Drag and drop the object via the inspector window.");
            }
            base.ComponentAwake();
        }

        /// <summary>
        /// Override this method  to add code for when this menu is opened
        /// </summary>
        public virtual void OnMenuOpened() { }

        /// <summary>
        /// Override this method to add code for when this menu is closed
        /// </summary>
        public virtual void OnMenuClosed() { }

    }
}
