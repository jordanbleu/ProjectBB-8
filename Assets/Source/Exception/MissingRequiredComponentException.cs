using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.BB8.Exception
{
    public class MissingRequiredComponentException : System.Exception
    {
        public MissingRequiredComponentException(GameObject gameObject, Type componentType) : base(GenerateMessage(gameObject.name, componentType.Name)) { }
        public MissingRequiredComponentException(GameObject gameObject, string componentName) : base(GenerateMessage(gameObject.name, componentName)) { }

        private static string GenerateMessage(string prefab, string component)
        {
            return $"Game Object '{prefab}' is missing a required component '{component}'";
        }

    }
}
