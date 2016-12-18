using System;

namespace NetProperty
{
    /// <summary>
    /// The exception that is thrown when a property either has an invalid name
    /// or is declared in an invalid way.
    /// </summary>
    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException(string msg)
            : base(msg)
        {
        }
    }
}
