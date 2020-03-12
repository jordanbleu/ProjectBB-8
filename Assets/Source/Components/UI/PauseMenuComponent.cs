using Assets.Source.Components.UI.Base;
using UnityEditor;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class PauseMenuComponent : MenuComponentBase
    {
        public void OnExitClicked()
        {
            // todo: this won't do anything in the final build.  
            // Tells the unity player to stop
            EditorApplication.isPlaying = false;
        }

        public void OnContinueClicked()
        {
            // todo:  This just restarts the current loaded scene for now
            menuSelector.CloseMenus();
        }

        public override void OnMenuOpened()
        {
            Time.timeScale = 0f;
            base.OnMenuOpened();
        }

        public override void OnMenuClosed()
        {
            Time.timeScale = 1f;
            base.OnMenuClosed();
        }
    }
}
