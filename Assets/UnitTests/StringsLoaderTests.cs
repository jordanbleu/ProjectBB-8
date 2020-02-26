using Assets.UnitTests.Mock.Strings;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    class StringsLoaderTests
    {
        [Test]
        public void CanLoadStringResources()
        {
            MockStringsLoader stringsLoader = new MockStringsLoader();
            stringsLoader.Load("strings.xml");
            AssertDictionaryContainsValue(stringsLoader.Value, "test1", "This is test 1");
            AssertDictionaryContainsValue(stringsLoader.Value, "test2", "This is test 2");
            AssertDictionaryContainsValue(stringsLoader.Value, "test3", "This is test 3");
        }

        private void AssertDictionaryContainsValue(Dictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (!dictionary[key].Equals(value))
                {
                    Assert.Fail($"Value of string '{key}' was '{dictionary[key]}' but we expected '{value}'");
                }
            }
            else
            {
                Assert.Fail($"Didn't load expected string '{key}'");
            }
        }
    }
}
