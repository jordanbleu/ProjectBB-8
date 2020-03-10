using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Source.TextWriterStyle.Factory.Base
{
    public abstract class TextWriterStyleFactoryBase
    {
        protected abstract Dictionary<string, Type> CommandRepository { get; }

        /// <summary>
        /// Creates an instance of the style command based on the name.  Then parses out the passed 
        /// in command arguments and prepares the instance for execution
        /// </summary>
        /// <returns>Fully Hydrated Style object</returns>
        public TextWriterStyleBase Create(string commandString)
        {
            if (string.IsNullOrWhiteSpace(commandString))
            {
                throw new StyleParseException("Unable to parse empty command string.  Please ensure there are no instnances of {} in the file.");
            }

            string[] commandStringParts = commandString.Split(':');

            if (commandStringParts.Count() > 2)
            {
                throw new StyleParseException($"Too many instances of ':' found in command string: '{commandString}'");
            }

            string command = commandStringParts[0].ToLower();
            string args = string.Empty;

            if (commandStringParts.Count() > 1)
            {
                args = commandStringParts[1];
            }

            if (!CommandRepository.ContainsKey(command))
            {
                throw new StyleParseException($"Unrecognized command, '{command}'");
            }

            Type commandType = CommandRepository[command];

            if (!commandType.IsSubclassOf(typeof(TextWriterStyleBase)))
            {
                throw new StyleParseException($"Command type for '{command}' is typeof '{commandType.GetType()}', " +
                    $"which does not inherit from {nameof(TextWriterStyleBase)}");
            }

            TextWriterStyleBase style = (TextWriterStyleBase)Activator.CreateInstance(commandType);
            style.Initialize(args);
            return style;
        }
    }
}
