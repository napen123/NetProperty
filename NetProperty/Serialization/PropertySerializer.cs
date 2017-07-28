using System;
using System.IO;
using System.Reflection;

namespace NetProperty.Serialization
{
    /// <summary>
    /// Serializes and deserializes objects to and from property files.
    /// </summary>
    public static class PropertySerializer
    {
        /// <summary>
        /// Serialize <paramref name="obj"/> and write it to a <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="file" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidFileNameChars" />.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="file"/> path exceeds the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters,
        /// and file names must be less than 260 characters.
        /// </exception>
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

                    string name, value;
                    var attr = field.GetCustomAttribute<PropertyAttribute>();

                    if (attr == null)
                    {
                        name = field.Name;
                        value = field.GetValue(obj).ToString();
                    }
                    else
                    {
                        name = attr.Name ?? field.Name;
                        value = attr.Converter != null
                            ? attr.Converter.Serialize(field.GetValue(obj))
                            : field.GetValue(obj).ToString();
                    }

                    WriteProperty(writer, name, value);
                }

                foreach (var property in type.GetProperties(flags))
                {
                    if (property.GetCustomAttribute<NonSerializedAttribute>() != null ||
                        property.GetSetMethod() == null)
                        continue;

                    string name, value;
                    var attr = property.GetCustomAttribute<PropertyAttribute>();

                    if (attr == null)
                    {
                        name = property.Name;
                        value = property.GetValue(obj).ToString();
                    }
                    else
                    {
                        name = attr.Name ?? property.Name;
                        value = attr.Converter != null
                            ? attr.Converter.Serialize(property.GetValue(obj))
                            : property.GetValue(obj).ToString();
                    }

                    WriteProperty(writer, name, value);
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
                var name = attr == null ? field.Name : (attr.Name ?? field.Name);
                var value = pFile[name];

                if (value == null)
                {
                    // TODO: Handle

                    continue;
                }

                field.SetValue(ret,
                    attr?.Converter != null
                        ? attr.Converter.Deserialize(value)
                        : Convert.ChangeType(value, field.FieldType));
            }
            
            foreach (var property in type.GetProperties(flags))
            {
                if (property.GetCustomAttribute<NonSerializedAttribute>() != null ||
                    property.GetSetMethod() == null)
                    continue;

                var attr = property.GetCustomAttribute<PropertyAttribute>();
                var name = attr == null ? property.Name : (attr.Name ?? property.Name);
                var value = pFile[name];
                
                if (value == null)
                {
                    // TODO: Handle

                    continue;
                }

                property.SetValue(ret,
                    attr?.Converter != null
                        ? attr.Converter.Deserialize(value)
                        : Convert.ChangeType(value, property.PropertyType));
            }

            return ret;
        }

        private static void WriteProperty(TextWriter writer, string name, string value)
        {
            if (value.Length > 0 && char.IsWhiteSpace(value[0]))
                writer.WriteLine($"{name} ~{value}");
            else
                writer.WriteLine($"{name} = {value}");
        }
    }
}
