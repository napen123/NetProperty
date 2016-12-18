using System;

namespace NetProperty
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PropertyName : Attribute
    {
        public string Name;

        public PropertyName(string name)
        {
            if(name.Contains("=") || name.Contains("~"))
                throw new InvalidPropertyException("Property names cannot contain \"=\" nor \"~\" : " + name);

            Name = name;
        }
    }
}
