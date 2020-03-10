using Assets.Source.TextWriterStyle.Base;
using System;
using System.Text;
using TMPro;

namespace Assets.UnitTests.Mock.TextWriterStyle
{
    public class HelloWorldArgStyle : TextWriterStyleBase
    {
        protected override void Initialize()
        {
            if (!ContainsArgument("FIRSTNAME"))
            {
                throw new InvalidOperationException("No argument for first");
            }

            if (!ContainsArgument("lastName"))
            {
                throw new InvalidOperationException("No argument for last name");
            }

            base.Initialize();
        }

        public override string Evaluate(TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            string firstName = GetArgumentValue("firstname");
            string lastName = GetArgumentValue("lastNAME");
            return $"Hello {firstName} {lastName}!";
        }
    }
}
