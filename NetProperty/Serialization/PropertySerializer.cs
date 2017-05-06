using System;
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
                {
                    if(field.GetCustomAttribute<NonSerializedAttribute>() != null)
                        continue;

                    WriteProperty(writer, field.GetCustomAttribute<PropertyAttribute>(), field.Name,
                        field.GetValue(obj));
                }

                foreach (var property in type.GetProperties(flags))
                {
                    if (property.GetCustomAttribute<NonSerializedAttribute>() != null ||
                        property.GetSetMethod() == null)
                        continue;
                    
                    WriteProperty(writer, property.GetCustomAttribute<PropertyAttribute>(), property.Name,
                        property.GetValue(obj));
                }
            }
        }

        /// <summary>
        /// Deserialize a <paramref name="file"/> into an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="file">The file to deserialize.</param>
        /// <returns>Returns a new instance of <typeparamref name="T"/>.</returns>
        public static T Deserialize<T>(string file)
            where T : new()
        {
            return Deserialize<T>(File.Open(file, FileMode.Open));
        }

        /// <summary>
        /// Deserialize a <paramref name="stream"/> into an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <returns>Returns a new instance of <typeparamref name="T"/>.</returns>
        public static T Deserialize<T>(Stream stream)
            where T : new()
        {
            var ret = new T();
            var type = ret.GetType();
            var pFile = new PropertyFile(stream);

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var field in type.GetFields(flags))
            {
                if (field.GetCustomAttribute<NonSerializedAttribute>() != null)
                    continue;

                var attr = field.GetCustomAttribute<PropertyAttribute>();
                var name = attr == null ? field.Name : attr.Name;
                var value = pFile[name];

                if (value == null)
                {
                    // TODO: Handle

                    continue;
                }
                
                field.SetValue(ret, Convert.ChangeType(value, field.FieldType));
            }
            
            foreach (var property in type.GetProperties(flags))
            {
                if (property.GetCustomAttribute<NonSerializedAttribute>() != null ||
                    property.GetSetMethod() == null)
                    continue;

                var attr = property.GetCustomAttribute<PropertyAttribute>();
                var name = attr == null ? property.Name : attr.Name;
                var value = pFile[name];
                
                if (value == null)
                {
                    // TODO: Handle

                    continue;
                }
                
                property.SetValue(ret, Convert.ChangeType(value, property.PropertyType));
            }

            return ret;
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
