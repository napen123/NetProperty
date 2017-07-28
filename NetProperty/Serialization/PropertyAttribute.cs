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
        /// <exception cref="ArgumentNullException"><paramref name="converter" /> is null.</exception>
        /// <exception cref="ArgumentException">The provided <paramref name="converter"/> is not a <see cref="PropertyConverter"/>.</exception>
        /// <exception cref="System.Reflection.TargetInvocationException">The constructor in <paramref name="converter"/> throws an exception.</exception>
        /// <exception cref="TypeLoadException"><paramref name="converter" /> is not a valid type. </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="converter" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
        /// -or-
        /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types,
        /// or arrays of those types, is not supported.
        /// -or-
        /// The assembly that contains <paramref name="converter" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.
        /// </exception>
        /// <exception cref="System.Runtime.InteropServices.COMException"><paramref name="converter" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="InvalidPropertyException">Property names cannot contain \"=\" or \"~\".</exception>
        public PropertyAttribute(string name, Type converter)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Property names cannot be null.");
            if (name.Contains("=") || name.Contains("~"))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;

            if (converter == null)
                Converter = null;
            else
            {
                var instance = Activator.CreateInstance(converter);

                if (instance is PropertyConverter pconv)
                    Converter = pconv;
                else
                    throw new ArgumentException("The provided converter is not a PropertyConverter.", nameof(converter));
            }

            Converter = converter == null ? null : Activator.CreateInstance(converter) as PropertyConverter;
        }
    }
}