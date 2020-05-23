using NUnit.Framework;
using Assets.Source.Extensions;

namespace UnitTests
{
    public class HelloWorldTests
    {
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Confirm that true == true, just in case
            Assert.IsTrue(true);
        }

        [Test]
        public void IsWithinTest() 
        {
            Assert.IsTrue(1f.IsWithin(1,2));
            Assert.IsTrue((-1f).IsWithin(1, 0));
            Assert.IsTrue((-1f).IsWithin(-1, 0));
        }

 
    }
}
