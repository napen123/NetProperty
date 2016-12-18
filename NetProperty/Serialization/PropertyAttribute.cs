using System;

namespace NetProperty.Serialization
{
    public class PropertyAttribute : Attribute
    {
        public string Name;

        public PropertyAttribute(string name)
        {
            if(name.Contains("=") || name.Contains("~"))
                throw new InvalidPropertyException("Property names cannot contain \"=\" or \"~\" : " + name);

            Name = name;
        }
    }
}