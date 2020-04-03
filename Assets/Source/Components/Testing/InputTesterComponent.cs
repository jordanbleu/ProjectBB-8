using Assets.Source.Components.Base;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.Testing
{
    public class InputTesterComponent : ComponentBase
    {
        private readonly List<string> bindings = new List<string>()
        {
            InputConstants.K_MENU_UP,
            InputConstants.K_MENU_DOWN,
            InputConstants.K_MENU_LEFT,
            InputConstants.K_MENU_RIGHT,
            InputConstants.K_MENU_ENTER,
            InputConstants.K_MENU_BACK,
            InputConstants.K_MOVE_LEFT,
            InputConstants.K_MOVE_RIGHT,
            InputConstants.K_MOVE_DOWN,
            InputConstants.K_MOVE_UP,
            InputConstants.K_ATTACK_PRIMARY,
            InputConstants.K_PAUSE,
            InputConstants.K_DASH_LEFT,
            InputConstants.K_DASH_RIGHT
        };

        [SerializeField]
        private TextMeshProUGUI textComponent;
        [SerializeField]
        private TextMeshProUGUI activeQueue;

        public override void ComponentStart()
        {
            if (textComponent == null)
            {
                throw new InvalidOperationException("Please drag a text mesh pro component into the 'Text Component' spot in the inspector");
            }
            
            base.ComponentStart();
        }

        public override void ComponentUpdate()
        {

            List<string> pressed = new List<string>();

            foreach (string binding in bindings)
            {
                pressed.Add(binding + ": " +
                    "Axis: " + InputManager.GetAxisValue(binding) +
                    ", IsKeyDown: " + InputManager.IsKeyHeld(binding));

                if (InputManager.IsKeyPressed(binding))
                {
                    Debug.Log($"Key Pressed: {binding}");
                }
                else if (InputManager.IsKeyReleased(binding))
                {
                    Debug.Log($"Key Released: {binding}");
                }
            }

            textComponent.SetText(string.Join("\n", pressed));
            activeQueue.SetText(InputManager.GetActiveListener().GetType().Name);


            //inputManager.Update();
            base.ComponentUpdate();
        }

    }
}
