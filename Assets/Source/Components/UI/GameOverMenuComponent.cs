using Assets.Source.Components.UI.Base;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets.Source.Components.UI
{
    public class GameOverMenuComponent : MenuComponentBase
    {
        public void OnExitClicked()
        {
            // todo: this won't do anything in the final build
            // Tells the unity player to stop
            EditorApplication.isPlaying = false;
        }

        public void OnRestartClicked()
        {
            // todo:  This just restarts the scene 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
