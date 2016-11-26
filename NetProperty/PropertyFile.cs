using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NetProperty
{
    /// <summary>
    /// Represents a property file.
    /// </summary>
    public class PropertyFile
    {
        /// <summary>
        /// The properties in the file.
        /// </summary>
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PropertyFile()
        {
        }

        /// <summary>
        /// Create a new property <paramref name="file"/> by loading an existing one.
        /// </summary>
        /// <param name="file">The property file to load.</param>
        public PropertyFile(string file)
        {
            Load(file);
        }

        /// <summary>
        /// Set a property's value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The property's new value.</param>
        public void SetProperty(string name, string value)
        {
            Properties[name] = value;
        }

        /// <summary>
        /// Get a property's value.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>If a property has that <paramref name="name"/>, return its value; otherwise, return <c>null</c>.</returns>
        public string GetProperty(string name)
        {
            return (from property in Properties where property.Key == name select property.Value).FirstOrDefault();
        }

        /// <summary>
        /// Load a property <paramref name="file"/>. If <paramref name="clearProperties"/> is <c>true</c>,
        /// clear/remove all existing <see cref="Properties"/>; if <c>false</c>, add to the existing <see cref="Properties"/>.
        /// </summary>
        /// <remarks>
        /// A property's value will be overrided if there's a name clash (a read property has the same name).
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="clearProperties">Should existing <see cref="Properties"/> be cleared/removed?</param>
        public void Load(string file, bool clearProperties = true)
        {
            if (!File.Exists(file))
                throw new IOException("Property file does not exist : " + file);
            if (clearProperties)
                Properties.Clear();

            foreach (var line in File.ReadLines(file))
            {
                var trimmed = line.Trim();

                if (trimmed.Length == 0 || trimmed.StartsWith("#"))
                    continue;

                if (trimmed.Contains("="))
                {
                    var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                    var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();
                    
                    Properties[name] = value;
                }
                else if (trimmed.Contains("~"))
                {
                    var name = trimmed.Substring(0, trimmed.IndexOf("~", StringComparison.Ordinal)).TrimEnd();
                    var value = trimmed.Substring(trimmed.IndexOf("~", StringComparison.Ordinal) + 1);

                    Properties[name] = value;
                }
                else
                    throw new Exception("Expected either \'=\' or \'~\' : " + line);
            }
        }

        /// <summary>
        /// Save the <seealso cref="Properties"/> to a <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to save to.</param>
        public void Save(string file)
        {
            using (var writer = new StreamWriter(file))
            {
                foreach (var property in Properties)
                {
                    var value = property.Value;

                    if (string.IsNullOrEmpty(value))
                        writer.WriteLine(property.Key + " =");
                    else if (char.IsWhiteSpace(value[0]))
                        writer.WriteLine(property.Key + " ~" + value);
                    else
                        writer.WriteLine(property.Key + " = " + value);
                }
            }
        }

        /// <summary>
        /// Get and set a property's value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get { return GetProperty(name); }
            set { SetProperty(name, value); }
        }
    }
}
