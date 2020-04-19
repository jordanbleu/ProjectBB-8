using Assets.Source.TextWriterStyle.Base;
using System.Text;
using TMPro;

namespace Assets.Source.TextWriterStyle
{
    public class EndColorStyle : TextWriterStyleBase
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        public override string Evaluate(TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            // Appending the string instantly appends the text
            currentText.Append($"</color>");

            // Returning a string would add it to the typed text 
            return "";
        }

    }
}
