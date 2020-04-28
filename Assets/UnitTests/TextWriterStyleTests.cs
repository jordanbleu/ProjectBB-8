using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Exception;
using Assets.UnitTests.Mock.TextWriterStyle;
using NUnit.Framework;
using System;
using System.Text;

namespace UnitTests
{
    public class TextWriterStyleTests
    {
        [Test]
        public void TextWriterStyles_CanEvaluateStyle()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            TextWriterStyleBase command = mockFactory.Create("helloworld");
            string result = command.Evaluate(null, null, null, -1, null);
            Assert.AreEqual(result, "HELLO WORLD!");
        }

        [Test]
        public void TextWriterStyles_CanEvaluateStyleWithWackyExtraArgs()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            TextWriterStyleBase command = mockFactory.Create("helloworld:asdf=123");
            string result = command.Evaluate(null, null, null, -1, null);
            Assert.AreEqual(result, "HELLO WORLD!");
        }


        [Test]
        public void TextWriterStyles_CanEvaluateStyleWithRequiredArgs()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            TextWriterStyleBase command = mockFactory.Create("helloWorldArgs:firstName=Jordan,lastName=Bleu");
            string result = command.Evaluate(null, null, null, -1, null);
            Assert.AreEqual(result, "Hello Jordan Bleu!");
        }


        [Test]
        public void TextWriterStyles_CanValidateRequiredArgs()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            try
            {
                TextWriterStyleBase command = mockFactory.Create("helloWorldArgs:firstName=Jordan");
            }
            catch (InvalidOperationException)
            {
                Assert.Pass();
            }
            // We shouldn't get this far because we're totes missing args
            Assert.Fail("Didn't catch any exceptions but we should have.");
        }

        
        [Test]
        public void ColorStyle_Works()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            string hex = "#AABBCC";

            TextWriterStyleBase command = mockFactory.Create($"color:hex={hex}");
            
            // This style should append to the builder not the text
            StringBuilder builder = new StringBuilder();

            string result = command.Evaluate(null, null, builder, -1, null);            
            Assert.AreEqual(builder.ToString(), $"<{hex}>");       
        }

        /// <summary>
        /// Tests that the color hex validation works 
        /// </summary>
        [Test]
        public void ColorStyle_FailsSuccessfully()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            string hex = "#poop";

            Assert.Throws<StyleValidationException>(() =>
            {
                TextWriterStyleBase command = mockFactory.Create($"color:hex={hex}");
            });
        }

        [Test]
        public void EndColorStyle_Works()
        {
            MockTextWriterStyleFactory mockFactory = new MockTextWriterStyleFactory();

            TextWriterStyleBase command = mockFactory.Create("endcolor");

            StringBuilder sb = new StringBuilder();

            string result = command.Evaluate(null, null, sb, -1, null);
            Assert.AreEqual(sb.ToString(), "</color>");
        }
    }
}
