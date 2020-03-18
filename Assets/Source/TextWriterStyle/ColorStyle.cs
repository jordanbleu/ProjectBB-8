using Assets.Source.Extensions;
using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Exception;
using System.Text;
using TMPro;

namespace Assets.Source.TextWriterStyle
{
    public class ColorStyle : TextWriterStyleBase
    {
        protected override void Initialize()
        {
            if (!ContainsArgument("hex") || string.IsNullOrEmpty(GetArgumentValue("hex")))
            {
                throw new StyleValidationException("Expected 'hex' argument in format '#RRGGBB'");
            }
            else
            {
                string hex = GetArgumentValue("hex");
                if (!hex.StartsWith("#"))
                {
                    throw new StyleValidationException("Hex value should start with #");
                }
                else if (!hex.ContainsOnly("#ABCDEF1234567890"))
                {
                    throw new StyleValidationException($"Hex value '{hex}' was not in a proper hexadecimal format");
                }
            }
            base.Initialize();
        }


        public override string Evaluate(TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            string hex = GetArgumentValue("hex");

            // Appending the string instantly appends the text
            currentText.Append($"<{hex}>");

            // Returning a string would add it to the typed text 
            return "";
        }

    }
}
