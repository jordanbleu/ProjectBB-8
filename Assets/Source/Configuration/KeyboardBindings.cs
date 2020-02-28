using Assets.Source.Configuration.Base;
using Assets.Source.Input;
using Assets.Source.Input.Constants;
using Assets.Source.Input.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Configuration
{
    [Serializable]
    public class KeyboardBindings : ConfigurationBase, IBindings
    {
        public Dictionary<string, KeyCodeValue> Bindings { get; set; } = new Dictionary<string, KeyCodeValue>()
        {
            // Menu Controls
            { InputConstants.K_MENU_UP,      new KeyCodeValue(Stringify(KeyCode.UpArrow))    },
            { InputConstants.K_MENU_DOWN,    new KeyCodeValue(Stringify(KeyCode.DownArrow))  },
            { InputConstants.K_MENU_LEFT,    new KeyCodeValue(Stringify(KeyCode.LeftArrow))  },
            { InputConstants.K_MENU_RIGHT,   new KeyCodeValue(Stringify(KeyCode.RightArrow)) },
            { InputConstants.K_MENU_ENTER,   new KeyCodeValue(Stringify(KeyCode.Return))     },
            { InputConstants.K_MENU_BACK,    new KeyCodeValue(Stringify(KeyCode.Escape))     },

            // Player Controls
            { InputConstants.K_MOVE_LEFT,   new KeyCodeValue(Stringify(KeyCode.A))      },
            { InputConstants.K_MOVE_RIGHT,  new KeyCodeValue(Stringify(KeyCode.D))      },
            { InputConstants.K_MOVE_DOWN,   new KeyCodeValue(Stringify(KeyCode.S))      },
            { InputConstants.K_MOVE_UP,     new KeyCodeValue(Stringify(KeyCode.W))      },
            { InputConstants.K_SHOOT,       new KeyCodeValue(Stringify(KeyCode.Space))  },
        };

        private static string Stringify(KeyCode key)
        {
            return ((int)key).ToString();
        }
    }
}
