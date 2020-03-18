namespace Assets.Source.TextWriterStyle.Exception
{
    public class StyleValidationException : System.Exception
    {
        public StyleValidationException(string message) : base(message)
        {
        }

        public StyleValidationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public StyleValidationException()
        {
        }
    }
}
