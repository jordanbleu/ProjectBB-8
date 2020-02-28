using Assets.Source.Configuration;
using System;
using UnityEngine;

namespace Assets.Source.Application
{
    public class Application
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Startup()
        {
            ConfigurationRepository.RefreshAll();
        }

    }
}
