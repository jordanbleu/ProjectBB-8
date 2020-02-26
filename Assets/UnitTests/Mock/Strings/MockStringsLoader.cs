using Assets.Source.Strings.Base;
using UnityEngine;

namespace Assets.UnitTests.Mock.Strings
{
    public class MockStringsLoader : StringsLoaderBase
    {
        protected override string GetLanguageCode()
        {
            return "en";
        }

        protected override string GetStringsDir()
        {
            return Application.dataPath + "/UnitTests/_testResources/Strings";
        }
    }
}
