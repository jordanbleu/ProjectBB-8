using Assets.Source.Components.TextWriter;
using Assets.Source.TextWriterStyle.Exception;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

namespace Assets.Source.TextWriterStyle.Base
{
    public abstract class TextWriterStyleBase
    {
        /// <summary>
        /// The parsed out arguments from the command string
        /// </summary>
        private Dictionary<string, string> arguments = new Dictionary<string, string>();

        /// <summary>
        /// Override this method to perform whatever action this command should be doing.  The string that is 
        /// returned will be added to the text mesh component
        /// </summary>
        /// <returns>String that will be conatenated by the magic type writer</returns>
        public abstract string Evaluate(TextWriterComponent textWriter, TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText);

        /// <summary>
        /// Override this method to check for the existence / validitity of our parsed <seealso cref="arguments"/>.
        /// Can also perform initialization logic.  Does nothing by default.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Parses all the arguments and prepares this command
        /// </summary>
        public void Initialize(string arguments)
        {
            ParseArguments(arguments);
            Initialize();
        }

        // Takes in an argument string (everything after the : and before the } ) and parses them 
        private void ParseArguments(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                // Example:
                // argument2 =blah, argument3 = blahs

                // break the arguments and values into chunks
                string[] argumentChunks = args.Split(',');

                foreach (string chunk in argumentChunks)
                {
                    // now split the chunk on the = sign
                    string[] parts = chunk.Split('=');

                    if (parts.Count() > 2)
                    {
                        throw new StyleParseException($"Unable to parse arguments '{args}'.  " +
                            $"Found multiple equal signs in an argument / value pair.");
                    }


                    string argument = parts[0].Trim().ToLower();
                    string value = string.Empty;

                    // its totally okay if we don't have a =
                    if (parts.Count() > 1)
                    {
                        value = parts[1].Trim();
                    }

                    if (arguments.ContainsKey(argument))
                    {
                        throw new StyleParseException($"Unable to parse arguments '{args}'.  Multiple instances of '{argument}'");
                    }

                    arguments.Add(argument, value);
                }
            }
        }

        /// <summary>
        /// Use to check for the existence of an argument
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        protected bool ContainsArgument(string argument)
        {
            return arguments.ContainsKey(argument.ToLower());
        }

        /// <summary>
        /// Gets the requested argument.  If it does not exist, an exception will be thrown
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        protected string GetArgumentValue(string argument)
        {
            if (!ContainsArgument(argument))
            {
                string argumentKeys = string.Join(", ", arguments.Keys);
                throw new StyleValidationException($"Unable to find argument for {argument}. " +
                    $"Discovered arguments are: {argumentKeys}");
            }

            return arguments[argument.ToLower()];
        }

        /// <summary>
        /// Returns the specified arguments
        /// </summary>
        protected IEnumerable<string> GetArgumentKeys()
        {
            return arguments.Keys;
        }
    }
}
