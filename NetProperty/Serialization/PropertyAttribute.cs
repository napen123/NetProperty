using System;

namespace NetProperty.Serialization
{
    /// <summary>
    /// Determines how a property should be serialized and deserialized.
    /// </summary>
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// The name the property has when serialized and deserialized.
        /// If <c>null</c>, the field/property name is used.
        /// </summary>
        public string Name;

        /// <summary>
        /// Constructor to set <see cref="Name"/>.
        /// </summary>
        /// <param name="name">The name to use (<seealso cref="Name"/>).</param>
        public PropertyAttribute(string name)
        {
            if (name != null && (name.Contains("=") || name.Contains("~")))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;
        }
    }
}