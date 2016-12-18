using System;

namespace NetProperty
{
    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException(string msg)
            : base(msg)
        {
        }
    }
}
