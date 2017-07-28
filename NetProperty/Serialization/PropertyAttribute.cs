using System;

namespace NetProperty.Serialization
{
    /// <summary>
    /// Used to provide additional information when serializing a field/property,
    /// or when deserializing a property into a field/property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// The converter to use when serializing and deserializing.
        /// </summary>
        public PropertyConverter Converter;

        private string _name;

        /// <summary>
        /// The name the property has when serialized and deserialized.
        /// Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException" accessor="set">Property names cannot be null.</exception>
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value), "Property names cannot be null.");
        }

        /// <summary>
        /// Initializes a new instance of PropertyAttribute with a provided <paramref name="name"/> and
        /// initialize <see cref="Converter"/> to <c>null</c>.
        /// </summary>
        /// <param name="name">The name to use.</param>
        public PropertyAttribute(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of PropertyAttribute with a provided <paramref name="name"/> and a <paramref name="converter"/>.
        /// </summary>
        /// <param name="name">The name to use.</param>
        /// <param name="converter">The converter to use.</param>
        public PropertyAttribute(string name, Type converter)
        {
            if(name == null)
                throw new InvalidPropertyException("Property names cannot be null.");
            if (name.Contains("=") || name.Contains("~"))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;
            Converter = converter == null ? null : Activator.CreateInstance(converter) as PropertyConverter;
        }
    }
}