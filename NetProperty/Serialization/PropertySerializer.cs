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
        /// Serialize <paramref name="obj"/> and write it to a <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void Serialize(string file, object obj)
        {
            Serialize(File.Open(file, FileMode.Create), obj);
        }

        /// <summary>
        /// Serialize <paramref name="obj"/> and write it to a <paramref name="stream"/>.
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
                    WriteProperty(writer, field.GetCustomAttribute<PropertyAttribute>(), field.Name, field.GetValue(obj));

                foreach (var property in type.GetProperties(flags))
                    WriteProperty(writer, property.GetCustomAttribute<PropertyAttribute>(), property.Name, property.GetValue(obj));
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

        private static void WriteProperty(TextWriter writer, PropertyAttribute attr, string orig, object obj)
        {
            if (obj == null)
                return;

            var name = attr == null ? orig : (attr.Name ?? orig);
            var value = obj.ToString();

            if (value.Length > 0 && char.IsWhiteSpace(value[0]))
                writer.WriteLine($"{name} ~{value}");
            else
                writer.WriteLine($"{name} = {value}");
        }
    }
}
