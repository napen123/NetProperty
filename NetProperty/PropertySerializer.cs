using System.IO;
using System.Reflection;

namespace NetProperty
{
    public static class PropertySerializer
    {
        public static void Serialize(Stream stream, object obj)
        {
            using (var writer = new StreamWriter(stream))
            {
                var type = obj.GetType();
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

                foreach (var field in type.GetFields(flags))
                {
                    var attr = field.GetCustomAttribute<PropertyAttribute>();
                    var value = field.GetValue(obj);

                    if(value == null)
                        continue;

                    var str = value.ToString();
                    
                    if (str.Length > 0 && char.IsWhiteSpace(str[0]))
                        writer.WriteLine(attr == null ? $"{field.Name} ~{str}" : $"{attr.Name} ~{str}");
                    else
                        writer.WriteLine(attr == null ? $"{field.Name} = {str}" : $"{attr.Name} = {str}");
                }

                foreach (var property in type.GetProperties(flags))
                {
                    var attr = property.GetCustomAttribute<PropertyAttribute>();
                    var value = property.GetValue(obj);

                    if (value == null)
                        continue;

                    var str = value.ToString();
                    
                    if (str.Length > 0 && char.IsWhiteSpace(str[0]))
                        writer.WriteLine(attr == null ? $"{property.Name} ~{str}" : $"{attr.Name} ~{str}");
                    else
                        writer.WriteLine(attr == null ? $"{property.Name} = {str}" : $"{attr.Name} = {str}");
                }
            }
        }

        public static PropertyFile Deserialize(Stream stream)
        {
            return new PropertyFile(stream);
        }
    }
}
