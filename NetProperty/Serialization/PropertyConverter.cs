namespace NetProperty.Serialization
{
    /// <summary>
    /// Used to convert types into a custom format when serializing and deserializing.
    /// </summary>
    public abstract class PropertyConverter
    {
        /// <summary>
        /// Serialize <paramref name="value"/> into a property value.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>The string representation of <paramref name="value"/>.</returns>
        public abstract string Serialize(object value);

        /// <summary>
        /// Deserialize <paramref name="value"/> into an object.
        /// </summary>
        /// <param name="value">The value to deserialize.</param>
        /// <returns>The <paramref name="value"/> in the form of an object.</returns>
        public abstract object Deserialize(string value);
    }
}
