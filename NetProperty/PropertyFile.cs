using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NetProperty
{
    /// <summary>
    /// Represents a property file.
    /// </summary>
    public class PropertyFile
    {
        /// <summary>
        /// The file's property groups.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Groups;
        
        /// <summary>
        /// The file's global properties.
        /// </summary>
        public Dictionary<string, string> GlobalProperties;
        
        /// <summary>
        /// Initializes a new instance of PropertyFile with no global properties and no groups.
        /// </summary>
        public PropertyFile()
        {
            Groups = new Dictionary<string, Dictionary<string, string>>();
            GlobalProperties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile class by loading a given property <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The property file to load.</param>
        public PropertyFile(string file)
            : this()
        {
            Load(file);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile class by loading from a given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        public PropertyFile(Stream stream)
            : this()
        {
            Load(stream);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile class given <paramref name="groups"/> and global <paramref name="properties"/>.
        /// </summary>
        /// <param name="groups">The groups to initialize with.</param>
        /// <param name="properties">The global properties to initialize with.</param>
        public PropertyFile(Dictionary<string, Dictionary<string, string>> groups,
            Dictionary<string, string> properties)
        {
            Groups = groups;
            GlobalProperties = properties;
        }

        /// <summary>
        /// Set a group's properties. If the group doesn't exist, create it.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="value">The group's properties.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null.</exception>
        public void SetGroupProperties(string name, Dictionary<string, string> value)
        {
            if (Groups == null)
                return;
            
            Groups[name] = value;
        }

        /// <summary>
        /// Get a group's properties. If the group doesn't exist, return null.
        /// </summary>
        /// <param name="name">The group's name.</param>
        /// <returns>If a group exists with the given <paramref name="name"/>, return its properties; otherwise, return null.</returns>
        public Dictionary<string, string> GetGroupProperties(string name)
        {
            if (Groups == null)
                return null;
            
            return (from @group in Groups where @group.Key == name select @group.Value).FirstOrDefault();
        }

        /// <summary>
        /// Set a property's value. If the property doesn't exist, create it.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The property's value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is null.</exception>
        public void SetGlobalProperty(string name, string value)
        {
            if (GlobalProperties == null)
                return;
            
            GlobalProperties[name] = value;
        }

        /// <summary>
        /// Get a property's value. If the property doesn't exist, return null.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>If a property exists with the given <paramref name="name"/>, return its value; otherwise, return null.</returns>
        public string GetGlobalProperty(string name)
        {
            if (GlobalProperties == null)
                return null;
            
            return (from property in GlobalProperties where property.Key == name select property.Value).FirstOrDefault();
        }

        /// <summary>
        /// Load a property <paramref name="file"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing global properties and groups; if <c>false</c>, append to the existing global properties
        /// and groups.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// <br />
        /// The <paramref name="file"/> will be opened as UTF-8; use <see cref="Load(string,Encoding,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="clearExisting">Should existing global properties and groups be removed before loading?</param>
        public void Load(string file, bool clearExisting = true)
        {
            Load(file, Encoding.UTF8, clearExisting);
        }

        /// <summary>
        /// Load a property file from a <paramref name="stream"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing global properties and groups; if <c>false</c>, append to the existing global properties
        /// and groups.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br />
        /// The <paramref name="stream"/> will be opened as UTF-8; use <see cref="Load(Stream,Encoding,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="clearExisting">Should existing global properties and groups be removed before loading?</param>
        public void Load(Stream stream, bool clearExisting = true)
        {
            Load(stream, Encoding.UTF8, clearExisting);
        }

        /// <summary>
        /// Load a property <paramref name="file"/> in the specified <paramref name="encoding"/>. If <paramref name="clearExisting"/> is <c>true</c>,
        /// remove all existing global properties and groups; if <c>false</c>, append to the existing global properties
        /// and groups.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Should existing global properties and groups be removed before loading?</param>
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
        /// remove all existing global properties and groups; if <c>false</c>, append to the existing global properties
        /// and groups.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encoding">The encoding to use when opening the file.</param>
        /// <param name="clearExisting">Should existing global properties and groups be removed before loading?</param>
        /// <exception cref="IOException" />
        /// <exception cref="InvalidPropertyException">Thrown when a property is declared incorrectly.</exception>
        public void Load(Stream stream, Encoding encoding, bool clearExisting = true)
        {
            if(Groups == null)
                Groups = new Dictionary<string, Dictionary<string, string>>();
            else if(clearExisting)
                Groups.Clear();
            
            if(GlobalProperties == null)
                GlobalProperties = new Dictionary<string, string>();
            else if(clearExisting)
                GlobalProperties.Clear();
            
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                string currentGroup = null;
                
                while ((line = reader.ReadLine()) != null)
                {
                    var trimmed = line.TrimStart();

                    if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal))
                        continue;

                    if (trimmed.StartsWith("["))
                    {
                        var closeIndex = trimmed.LastIndexOf(']');

                        if (closeIndex == -1)
                            throw new PropertyGroupException("Group declaration missing closing bracket (']').");

                        currentGroup = trimmed.Substring(1, closeIndex - 1).Trim();

                        if (currentGroup.Length == 0)
                            currentGroup = null;
                        else
                            Groups.Add(currentGroup, new Dictionary<string, string>());
                    }
                    else
                    {
                        if (trimmed.Contains("="))
                        {
                            var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                            var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();

                            if (currentGroup == null)
                                GlobalProperties[name] = value;
                            else
                                Groups[currentGroup][name] = value;
                        }
                        else if (trimmed.Contains("~"))
                        {
                            var name = trimmed.Substring(0, trimmed.IndexOf("~", StringComparison.Ordinal)).TrimEnd();
                            var value = trimmed.Substring(trimmed.IndexOf("~", StringComparison.Ordinal) + 1);

                            if (currentGroup == null)
                                GlobalProperties[name] = value;
                            else
                                Groups[currentGroup][name] = value;
                        }
                        else
                            throw new InvalidPropertyException("Expected either \'=\' or \'~\' : " + line);
                    }
                }
            }
        }

        /// <summary>
        /// Save the global properties and groups to a given <paramref name="file"/>.
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
        /// Save the global properties and groups to a given <paramref name="stream"/>.
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
        /// Save the global properties and groups to a given <paramref name="file"/>, in a given <paramref name="encoding"/>.
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
        /// Save the global properties and groups to a given <paramref name="stream"/>, in a given <paramref name="encoding"/>.
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
                
                if (GlobalProperties != null && GlobalProperties.Count > 0)
                    WriteProperties(GlobalProperties);
                if (Groups == null || Groups.Count <= 0)
                    return;
                
                foreach (var group in Groups)
                {
                    writer.WriteLine("[" + @group.Key + "]");
                    WriteProperties(@group.Value);
                }
            }
        }

        /// <summary>
        /// Get or set a group's properties.
        /// </summary>
        /// <param name="name">The group's name.</param>
        /// <returns>Returns the group's properties; if it doesn't exist, returns null.</returns>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="name" /> is null.</exception>
        public Dictionary<string, string> this[string name]
        {
            get => GetGroupProperties(name);
            set => SetGroupProperties(name, value);
        }
    }
}
