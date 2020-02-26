using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.BB8.Exception
{
    public class MissingResourceException : System.Exception
    {
        public MissingResourceException(string path) : base(GenerateMessage(path)) { }

        private static string GenerateMessage(string path)
        {
            return $"Missing resource for path: '{path}'.";
        }

        public MissingResourceException()
        {
        }

        public MissingResourceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
