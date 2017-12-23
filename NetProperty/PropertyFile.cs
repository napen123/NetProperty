using System;
using System.IO;
using System.Text;
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
        /// The file's properties.
        /// </summary>
        public Dictionary<string, string> Properties;
        
        /// <summary>
        /// Initializes a new instance of PropertyFile with no properties.
        /// </summary>
        public PropertyFile()
        {
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading properties from a <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        public PropertyFile(string file)
            : this()
        {
            Load(file);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        public PropertyFile(Stream stream)
            : this()
        {
            Load(stream);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile with an initial starting capacity.
        /// </summary>
        /// <see cref="Dictionary{TKey,TValue}(int)"/>
        /// <param name="capacity">The properties to initialize with.</param>
        public PropertyFile(int capacity)
        {
            Properties = new Dictionary<string, string>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile with given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">The properties to initialize with.</param>
        public PropertyFile(Dictionary<string, string> properties)
        {
            Properties = properties;
        }
        
        /// <summary>
        /// Set a property's value. If the property doesn't exist, create it.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The property's value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null.</exception>
        public void SetProperty(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException();

            Properties[name] = value;
        }

        /// <summary>
        /// Get a property's value. If the property doesn't exist, return null.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>If a property has the given <paramref name="name"/>, return its value; otherwise, return null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null.</exception>
        public string GetProperty(string name)
        {
            if (name == null)
                throw new ArgumentNullException();
            
            return (from property in Properties where property.Key == name select property.Value).FirstOrDefault();
        }

        /// <summary>
        /// Load a property <paramref name="file"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing properties; if <c>false</c>, append to the properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// <br />
        /// The <paramref name="file"/> will be opened as UTF-8; use <see cref="Load(string,Encoding,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        public void Load(string file, bool clearExisting = true)
        {
            Load(file, Encoding.UTF8, clearExisting);
        }

        /// <summary>
        /// Load a property file from a <paramref name="stream"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing properties; if <c>false</c>, append to the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br />
        /// The <paramref name="stream"/> will be opened as UTF-8; use <see cref="Load(Stream,Encoding,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        public void Load(Stream stream, bool clearExisting = true)
        {
            Load(stream, Encoding.UTF8, clearExisting);
        }

        /// <summary>
        /// Load a property <paramref name="file"/> in the specified <paramref name="encoding"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing properties; if <c>false</c>, append to the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <exception cref="ArgumentException"><paramref name="file" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidFileNameChars" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null. </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <seealso cref="file"/> path exceeds the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters,
        /// and file names must be less than 260 characters.
        /// </exception>
        public void Load(string file, Encoding encoding, bool clearExisting = true)
        {
            Load(File.Open(file, FileMode.Open), encoding, clearExisting);
        }

        /// <summary>
        /// Load a property file from a <paramref name="stream"/> in the specified <paramref name="encoding"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing properties; if <c>false</c>, append to the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encoding">The encoding to use when opening the file.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <exception cref="IOException" />
        /// <exception cref="InvalidPropertyException">Thrown when a property is declared incorrectly.</exception>
        public void Load(Stream stream, Encoding encoding, bool clearExisting = true)
        {
            if(Properties == null)
                Properties = new Dictionary<string, string>();
            else if(clearExisting)
                Properties.Clear();
            
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var trimmed = line.TrimStart();

                    if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal))
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
                        throw new InvalidPropertyException("Expected either \'=\' or \'~\' : " + line);
                }
            }
        }

        /// <summary>
        /// Save <see cref="Properties"/> to a given <paramref name="file"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="file"/> is saved as UTF-8; use <see cref="Save(string,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The file to save to.</param>
        public void Save(string file)
        {
            Save(file, Encoding.UTF8);
        }

        /// <summary>
        /// Save <see cref="Properties"/> to a given <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="stream"/> is saved as UTF-8; use <see cref="Save(Stream,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to save to.</param>
        public void Save(Stream stream)
        {
            Save(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Save <see cref="Properties"/> to a given <paramref name="file"/>, in a given <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to save to.</param>
        /// <param name="encoding">The encoding to save as.</param>
        /// <exception cref="IOException" />
        /// <exception cref="ArgumentException"><paramref name="file" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidFileNameChars" />. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null. </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="file"/> path exceeds the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters,
        /// and file names must be less than 260 characters.
        /// </exception>
        public void Save(string file, Encoding encoding)
        {
            Save(File.Open(file, FileMode.Create), encoding);
        }

        /// <summary>
        /// Save <see cref="Properties"/> to a given <paramref name="stream"/>, in a given <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="encoding">The encoding to save as.</param>
        /// <exception cref="IOException" />
        /// <exception cref="ArgumentException"><paramref name="stream" /> is not writable. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> or <paramref name="encoding" /> is null. </exception>
        public void Save(Stream stream, Encoding encoding)
        {
            using (var writer = new StreamWriter(stream, encoding))
            {
                void WriteProperties(Dictionary<string, string> properties)
                {
                    foreach (var property in properties)
                    {
                        var value = property.Value;
                    
                        if (!string.IsNullOrEmpty(value) && char.IsWhiteSpace(value[0]))
                            writer.WriteLine(property.Key + " ~" + value);
                        else
                            writer.WriteLine(property.Key + " = " + value);
                    }
                }
                
                if (Properties != null && Properties.Count > 0)
                    WriteProperties(Properties);
            }
        }

        /// <summary>
        /// Get or set a property's value.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>Returns the property's value; if the property doesn't exist, returns null.</returns>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="name" /> is null.</exception>
        public string this[string name]
        {
            get => GetProperty(name);
            set => SetProperty(name, value);
        }
    }
}
