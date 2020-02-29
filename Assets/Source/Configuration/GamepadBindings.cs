using Assets.Source.Configuration.Base;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using Assets.Source.Input.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Source.Configuration
{
    [Serializable]
    public class GamepadBindings : ConfigurationBase, IBindings
    {
        public Dictionary<string, KeyCodeValue> Bindings { get; set; } = new Dictionary<string, KeyCodeValue>()
        {
            // Menu Controls
            { InputConstants.K_MENU_UP,     new KeyCodeValue(GamepadConstants.GP_DPAD_V, KeyCodeValue.AxisDirections.Positive) },
            { InputConstants.K_MENU_DOWN,   new KeyCodeValue(GamepadConstants.GP_DPAD_V, KeyCodeValue.AxisDirections.Negative) },
            { InputConstants.K_MENU_LEFT,   new KeyCodeValue(GamepadConstants.GP_DPAD_H, KeyCodeValue.AxisDirections.Negative) },
            { InputConstants.K_MENU_RIGHT,  new KeyCodeValue(GamepadConstants.GP_DPAD_H, KeyCodeValue.AxisDirections.Positive) },
            { InputConstants.K_MENU_ENTER,  new KeyCodeValue(GamepadConstants.GP_ABUTTON) },
            { InputConstants.K_MENU_BACK,   new KeyCodeValue(GamepadConstants.GP_BBUTTON) },

            // Player Controls
            { InputConstants.K_MOVE_LEFT,       new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Positive) },
            { InputConstants.K_MOVE_RIGHT,      new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_H, KeyCodeValue.AxisDirections.Negative) },
            { InputConstants.K_MOVE_DOWN,       new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Negative) },
            { InputConstants.K_MOVE_UP,         new KeyCodeValue(GamepadConstants.GP_LEFTSTICK_V, KeyCodeValue.AxisDirections.Positive) },
            { InputConstants.K_ATTACK_PRIMARY,  new KeyCodeValue(GamepadConstants.GP_ABUTTON) },
            { InputConstants.K_PAUSE,           new KeyCodeValue(GamepadConstants.GP_START_BUTTON) },
            { InputConstants.K_DASH_LEFT,       new KeyCodeValue(GamepadConstants.GP_LEFTBUMPER) },
            { InputConstants.K_DASH_RIGHT,      new KeyCodeValue(GamepadConstants.GP_RIGHTBUMPER) },
        };
    }
}
