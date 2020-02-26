using Assets.Source.Configuration;
using Assets.Source.Configuration.Exception;
using Assets.Source.Input.Interfaces;
using System;
using UnityEngine;

namespace Assets.Source.Input
{
    public class KeyboardInputListener : IInputListener
    {
        private IBindings bindings;

        public IBindings GetKeyBindings()
        {
            if (bindings == null)
            {
                bindings = ConfigurationRepository.KeyboardBindings;
            }
            return bindings;
        }

        public float GetAxis(string binding)
        {
            KeyCode keyCode = GetKeyCode(binding);

            return UnityEngine.Input.GetKey(keyCode) ? 1 : 0;

        }

        public bool IsKeyDown(string binding)
        {
            return UnityEngine.Input.GetKey(GetKeyCode(binding));
        }

        public bool IsKeyHit(string binding)
        {
            return UnityEngine.Input.GetKeyDown(GetKeyCode(binding));
        }

        public bool IsKeyReleased(string binding)
        {
            return UnityEngine.Input.GetKeyUp(GetKeyCode(binding));
        }

        public bool IsNeutral()
        {
            foreach (string binding in GetKeyBindings().Bindings.Keys)
            {
                if (GetAxis(binding) > 0f)
                {
                    return false;
                }
            }
            return true;
        }

        private KeyCode GetKeyCode(string binding)
        {
            if (GetKeyBindings().Bindings.TryGetValue(binding, out KeyCodeValue keyCodeValue))
            {
                if (Enum.TryParse(keyCodeValue.KeyCode, out KeyCode keyCode))
                {
                    return keyCode;
                }
                throw new InvalidConfigurationException<KeyboardBindings>
                    ($"Key binding value '{keyCodeValue.KeyCode}' is not a valid keyboard KeyCode.");
            }
            throw new InvalidConfigurationException<KeyboardBindings>($"Unable to find key binding for {binding}");
        }

    }
}
