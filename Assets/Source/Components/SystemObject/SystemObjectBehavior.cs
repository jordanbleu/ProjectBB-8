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
            // todo:  This should be set via a settings value (trello card exists)
            //InputManager = new InputManager(new KeyboardInputListener());

            // To Test gamepad input, uncomment this line **********************************************************************************************
            InputManager = new InputManager(new GamepadInputListener());
        }

        public void Update()
        {
            // This is a horrendous way to do this 
            // todo: this is a hack and should be fixed later
            if (InputManager.GetActiveListener() is GamepadInputListener gamepadInputListener)
            {
                gamepadInputListener.UpdateInputList();
            }
        }
    }
}
