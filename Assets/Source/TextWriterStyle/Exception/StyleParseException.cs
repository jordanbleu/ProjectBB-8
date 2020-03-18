namespace Assets.Source.TextWriterStyle.Exception
{
    public class StyleParseException : System.Exception
    {
        public StyleParseException(string message) : base(message)
        {
        }

        public StyleParseException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public StyleParseException()
        {
        }

    }
}
