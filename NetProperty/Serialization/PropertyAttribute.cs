using System;

namespace NetProperty.Serialization
{
    /// <summary>
    /// Determines how a property should be serialized and deserialized.
    /// </summary>
    public class PropertyAttribute : Attribute
    {
        public string Name;

        public PropertyAttribute(string name)
        {
            if (name != null && (name.Contains("=") || name.Contains("~")))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;
        }
    }
}