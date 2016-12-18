using System.IO;
using System.Reflection;

namespace NetProperty.Serialization
{
    /// <summary>
    /// Serializes and deserializes objects to and from files.
    /// </summary>
    public static class PropertySerializer
    {
        /// <summary>
        /// Serialize <paramref name="obj"/> and writes it to a <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void Serialize(string file, object obj)
        {
            Serialize(File.Open(file, FileMode.Create), obj);
        }

        /// <summary>
        /// Serialize <paramref name="obj"/> and writes it to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="obj">The object to serialize.</param>
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

        /// <summary>
        /// Deserialize a <paramref name="file"/> into a <see cref="PropertyFile"/>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to using one of the constructors/using one of the load methods.
        /// </remarks>
        /// <param name="file">The file to deserialize.</param>
        /// <returns>Returns a <see cref="PropertyFile"/>.</returns>
        public static PropertyFile Deserialize(string file)
        {
            return new PropertyFile(file);
        }

        /// <summary>
        /// Deserialize a <paramref name="stream"/> into a <see cref="PropertyFile"/>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to using one of the constructors/using one of the load methods.
        /// </remarks>
        /// <param name="stream">The stream to deserialize.</param>
        /// <returns>Returns a <see cref="PropertyFile"/>.</returns>
        public static PropertyFile Deserialize(Stream stream)
        {
            return new PropertyFile(stream);
        }
    }
}
