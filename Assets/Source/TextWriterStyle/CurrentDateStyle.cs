using Assets.Source.Components.TextWriter;
using Assets.Source.TextWriterStyle.Base;
using System;
using System.Text;
using TMPro;

namespace Assets.Source.TextWriterStyle
{
    public class CurrentDateStyle : TextWriterStyleBase
    {
        /// <summary>
        /// Returns the current date as a string. 
        /// <para>ARGUMENTS:</para>
        /// <para>format (OPTIONAL): the format string for the returned date.</para>
        /// </summary>
        /// <returns>a date string in the requested format (or the default format if non is specified)</returns>
        public override string Evaluate(TextWriterComponent textWriter, TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            if (ContainsArgument("format"))
            {
                return DateTime.Now.ToString(GetArgumentValue("format"));
            }
            return DateTime.Now.ToString();
        }
    }
}
