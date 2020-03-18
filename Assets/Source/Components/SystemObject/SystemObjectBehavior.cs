using Assets.Source.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.SystemObject
{
    /// <summary>
    /// This is the behavior used for the SystemObject.  The SystemObject handles
    /// globally available things such as input handling, etc
    /// <para>This object does not inherit from ReplicantBehavior, since that would cause a stackoverflow</para>
    /// </summary>
    public class SystemObjectBehavior : MonoBehaviour
    {

        /// <summary>
        /// Used for testing input from the active Input Listener (either keyboard / mouse or gamepad)
        /// </summary>
        public InputManager InputManager { get; private set; }


        public void Awake()
        {
            // todo:  This should be set via a settings value 
            InputManager = new InputManager(new KeyboardInputListener());

        }

        public void Update()
        {

            // todo:  Temporary hotkey to allow swapping inputs on the fly.  Pressing Ctrl + I swaps inputs
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                if (InputManager.GetActiveListener() is GamepadInputListener)
                {
                    Debug.Log("Swapping input mode to Keyboard");
                    InputManager = new InputManager(new KeyboardInputListener());
                }
                else
                {
                    Debug.Log("Swapping input mode to Gamepad");
                    InputManager = new InputManager(new GamepadInputListener());
                }
            }

            // todo: fix this
            if (InputManager.GetActiveListener() is GamepadInputListener gamepadInputListener)
            {
                gamepadInputListener.UpdateInputList();
            }
        }
    }
}
