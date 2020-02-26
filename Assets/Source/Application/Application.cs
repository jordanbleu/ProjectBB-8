using Assets.Source.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Application
{
    public class Application
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Startup()
        {
            // Temporary sanity check.
            Debug.Log($"Application started at {DateTime.Now}. ");

            ConfigurationRepository.RefreshAll();
        }

    }
}
