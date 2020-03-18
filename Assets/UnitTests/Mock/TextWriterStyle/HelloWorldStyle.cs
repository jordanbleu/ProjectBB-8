using Assets.Source.TextWriterStyle.Base;
using System;
using System.Text;
using TMPro;

namespace Assets.UnitTests.Mock.TextWriterStyle
{
    public class HelloWorldStyle : TextWriterStyleBase
    {
        public override string Evaluate(TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            return "HELLO WORLD!";
        }

    }
}
