using System;
using System.Runtime.Serialization;

namespace NetProperty
{
    /// <summary>
    /// Thrown when a property is declared in an invalid way.
    /// </summary>
    [Serializable]
    public class InvalidPropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of PropertyFile with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        public InvalidPropertyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile for deserialization,
        /// given <paramref name="info"/> and <paramref name="context"/>.
        /// </summary>
        /// <param name="info">The info to use when deserializing.</param>
        /// <param name="context">The context to use when deserializing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
        protected InvalidPropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
