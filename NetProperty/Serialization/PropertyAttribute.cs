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
        /// Cannot be null.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new InvalidPropertyException("Property names cannot be null.");
        }

        /// <summary>
        /// The converter to use when serializing and deserializing.
        /// </summary>
        public PropertyConverter Converter;

        private string _name;

        /// <summary>
        /// Initialize with a <paramref name="name"/> and null for the converter (<seealso cref="Converter"/>).
        /// </summary>
        /// <param name="name">The name to use (<seealso cref="Name"/>).</param>
        public PropertyAttribute(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initialize with a <paramref name="name"/> and, optionally, a <paramref name="converter"/>.
        /// </summary>
        /// <param name="name">The name to use (<seealso cref="Name"/>).</param>
        /// <param name="converter">The converter to use (<seealso cref="Converter"/>).</param>
        public PropertyAttribute(string name, Type converter)
        {
            if(name == null)
                throw new InvalidPropertyException("Property names cannot be null.");
            if (name.Contains("=") || name.Contains("~"))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;

            if (converter == null)
                Converter = null;
            else
                Converter = Activator.CreateInstance(converter) as PropertyConverter;
        }
    }
}