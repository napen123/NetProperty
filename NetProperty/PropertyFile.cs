using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace NetProperty
{
    /// <summary>
    /// Represents a property file.
    /// </summary>
    public class PropertyFile : ICollection<KeyValuePair<string, string>>
    {
        /// <summary>
        /// The file's properties.
        /// </summary>
        public Dictionary<string, string> Properties;
        
        /// <summary>
        /// Gets the number of properties in the file.
        /// </summary>
        public int Count => Properties.Count;

        /// <summary>
        /// Whether or not this is a read-only collection; always returns false.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of PropertyFile with no properties.
        /// </summary>
        public PropertyFile()
        {
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile with an initial starting <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The capacity to initialize with.</param>
        public PropertyFile(int capacity)
        {
            Properties = new Dictionary<string, string>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile with provided <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">The properties to initialize with.</param>
        public PropertyFile(Dictionary<string, string> properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="file" />.
        /// </summary>
        /// <remarks>
        /// The <paramref name="file"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(string,Encoding,bool)"/> to specify another.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(string file, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(file, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by trying to load as many properties from a <paramref name="file" /> as possible.
        /// </summary>
        /// <remarks>
        /// The <paramref name="file"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(string,Encoding,out bool,bool)"/> to specify another.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="result">True if all properties were successfully loaded; false if errors were encountered.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(string file, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(file, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by trying to load as many properties from a <paramref name="file" /> as possible,
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="result">True if all properties were successfully loaded; false if errors were encountered.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(string file, Encoding encoding, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(file, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="file" />
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(string file, Encoding encoding, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(file, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="stream" />.
        /// </summary>
        /// <remarks>
        /// The <paramref name="stream"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(Stream,Encoding,bool)"/> to specify another.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(Stream stream, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(stream, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="stream" />
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading from the <paramref name="stream"/>.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(Stream stream, Encoding encoding, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(stream, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by trying to load as many properties from a <paramref name="stream" /> as possible.
        /// </summary>
        /// <remarks>
        /// The <paramref name="stream"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(Stream,Encoding,out bool,bool)"/> to specify another.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="result">True if all properties were successfully loaded; false if errors were encountered.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(Stream stream, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(stream, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by trying to load as many properties from a <paramref name="stream" /> as possible,
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="stream"/>.</param>
        /// <param name="result">True if all properties were successfully loaded; false if errors were encountered.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public PropertyFile(Stream stream, Encoding encoding, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(stream, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Adds a property with a given <paramref name="name"/> and <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// If a property already exists with the provided <paramref name="name"/>,
        /// then it will be overridden instead.
        ///
        /// If <paramref name="name"/> is null, then this will return false.
        /// </remarks>
        /// <param name="name">The name of the property to add.</param>
        /// <param name="value">The value of the property to add.</param>
        /// <returns>Returns true if the value was successfully added; false if not.</returns>
        public bool Add(string name, string value)
        {
            if (name == null)
                return false;

            Properties[name] = value;

            return true;
        }
        
        /// <summary>
        /// Adds a property using the provided name-value <paramref name="pair"/>.
        /// </summary>
        /// <remarks>
        /// If a property with the provided name already exists,
        /// then it will be overridden instead.
        /// </remarks>
        /// <param name="pair">The name-value pair to create a property from.</param>
        /// <exception cref="ArgumentNullException">The provided property name is null.</exception>
        public void Add(KeyValuePair<string, string> pair)
        {
            Properties[pair.Key] = pair.Value;
        }

        /// <summary>
        /// Determines whether the PropertyFile contains a property with the specified <paramref name="name"/>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="name"/> is null, then this will return false.
        /// </remarks>
        /// <param name="name">The name to check for.</param>
        /// <returns>Returns true if a property has the specified <paramref name="name"/>; false if not.</returns>
        public bool Contains(string name)
        {
            return name != null && Properties.ContainsKey(name);
        }

        /// <summary>
        /// Determines whether or not a property has the provided <paramref name="name"/> and <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="name"/> is null, then this will return false.
        /// </remarks>
        /// <param name="name">The name to check for.</param>
        /// <param name="value">The value to check for.</param>
        /// <returns>Returns true if a property has the specified <paramref name="name"/> and <paramref name="value"/>; false if not.</returns>
        public bool Contains(string name, string value)
        {
            return name != null && Properties.TryGetValue(name, out var v) && v == value;
        }

        /// <summary>
        /// Determines whether or not a property has the provided name and value.
        /// </summary>
        /// <remarks>
        /// If the specified name is null, then this will return false.
        /// </remarks>
        /// <param name="pair">The name-value pair to check for.</param>
        /// <returns>Returns true if a property has the given name and value; false if not.</returns>
        public bool Contains(KeyValuePair<string, string> pair)
        {
            var name = pair.Key;

            return name != null && Properties.TryGetValue(name, out var value) && value == pair.Value;
        }

        /// <summary>
        /// Sets a property's value. If the property doesn't exist, then it will be created.
        /// </summary>
        /// <remarks>
        /// If <paramref name="name"/> is null, then this will return false.
        /// </remarks>
        /// <param name="name">The property's name.</param>
        /// <param name="value">The property's value.</param>
        /// <returns>Returns true if the property's value was successfully set; false if not.</returns>
        public bool SetProperty(string name, string value)
        {
            if (name == null)
                return false;

            Properties[name] = value;

            return true;
        }

        /// <summary>
        /// Gets a property's value based on its <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>Returns the property's value; null if it doesn't exist or <paramref name="name"/> was null.</returns>
        public string GetProperty(string name)
        {
            if (name == null || !Properties.TryGetValue(name, out var value))
                return null;

            return value;
        }

        /// <summary>
        /// Removes the property with the specified <paramref name="name"/>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="name"/> is null, then this will return false.
        /// </remarks>
        /// <param name="name">The name to check for.</param>
        /// <returns>Returns true if the property was removed; false if not.</returns>
        public bool Remove(string name)
        {
            return name != null && Properties.Remove(name);
        }

        /// <summary>
        /// Removes the property that matches the provided name-value <paramref name="pair"/>.
        /// </summary>
        /// <remarks>
        /// If the given name is null, then this will return false.
        /// </remarks>
        /// <param name="pair">The name-value pair to check for.</param>
        /// <returns>Returns true if the property was removed; false if not.</returns>
        public bool Remove(KeyValuePair<string, string> pair)
        {
            var name = pair.Key;
            
            if (name == null || !Properties.TryGetValue(name, out var value) || value != pair.Value)
                return false;

            Properties.Remove(name);
                
            return true;
        }

        /// <summary>
        /// Clears the file's <see cref="Properties"/>.
        /// </summary>
        public void Clear()
        {
            Properties.Clear();
        }

        /// <summary>
        /// Loads properties from a <paramref name="file"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br />
        /// <see cref="Properties"/> will be initialized if it's null.
        /// <br />
        /// The <paramref name="file"/> will be opened using UTF-8; use <see cref="Load(string,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public void Load(string file, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            Load(file, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Loads properties from a <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br />
        /// <see cref="Properties"/> will be initialized if it's null.
        /// <br />
        /// The <paramref name="stream"/> will be opened using UTF-8; use <see cref="Load(Stream,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public void Load(Stream stream, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            Load(stream, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Loads properties from a <paramref name="file"/> using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br />
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading from the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        public void Load(string file, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                Load(stream, encoding, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Loads properties from a <paramref name="stream"/> using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading from the <paramref name="stream"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        /// <exception cref="InvalidPropertyException">Thrown if a property is declared incorrectly (i.e. missing either a "=" or "~").</exception>
        public void Load(Stream stream, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            if (Properties == null)
                Properties = new Dictionary<string, string>();
            else if (clearExisting)
                Properties.Clear();

            using (var reader = new StreamReader(stream, encoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var trimmed = line.TrimStart();

                    if (trimmed.Length == 0 || trimmed[0] == '#')
                        continue;
                    
                    if (trimmed.Contains("="))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else if (trimmed.Contains("~"))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf('~')).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf('~') + 1);

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else
                        throw new InvalidPropertyException("Expected either \'=\' or \'~\' in property declaration : " + line);
                }
            }
        }

        /// <summary>
        /// Tries to load as many properties from a <paramref name="file"/> as possible.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// <br />
        /// The <paramref name="file"/> will be opened using UTF-8; use <see cref="TryLoad(string,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        /// <returns>Returns true if all properties were successfully loaded; false if errors were encountered.</returns>
        public bool TryLoad(string file, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            return TryLoad(file, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Tries to load as many properties from a <paramref name="stream"/> as possible.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// <br />
        /// The <paramref name="stream"/> will be opened using UTF-8; use <see cref="TryLoad(Stream,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        /// <returns>Returns true if all properties were successfully loaded; false if errors were encountered.</returns>
        public bool TryLoad(Stream stream, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            return TryLoad(stream, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Tries to load as many properties from a <paramref name="file"/> as possible,
        /// using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        /// <returns>Returns true if all properties were successfully loaded; false if errors were encountered.</returns>
        public bool TryLoad(string file, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                return TryLoad(stream, encoding, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Tries to load as many properties from a <paramref name="stream"/> as possible,
        /// using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Properties defined multiple times will have the last-defined value.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="stream"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty and whitespace-only values will be added as null.</param>
        /// <returns>Returns true if all properties were successfully loaded; false if errors were encountered.</returns>
        public bool TryLoad(Stream stream, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            if (Properties == null)
                Properties = new Dictionary<string, string>();
            else if (clearExisting)
                Properties.Clear();

            var ret = true;

            using (var reader = new StreamReader(stream, encoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var trimmed = line.TrimStart();

                    if (trimmed.Length == 0 || trimmed[0] == '#')
                        continue;

                    if (trimmed.Contains("="))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else if (trimmed.Contains("~"))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf('~')).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf('~') + 1);

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else
                        ret = false;
                }
            }

            return ret;
        }
        
        /// <summary>
        /// Saves <see cref="Properties"/> to a specified <paramref name="file"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// The <paramref name="file"/> is saved using UTF-8; use <see cref="Save(string,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The file to save to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is null.</exception>
        /// <exception cref="NullReferenceException"><see cref="Properties"/> is null.</exception>
        public void Save(string file)
        {
            Save(file, Encoding.UTF8);
        }

        /// <summary>
        /// Saves <see cref="Properties"/> to a specified <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// The <paramref name="stream"/> is saved using UTF-8; use <see cref="Save(Stream,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to save to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="NullReferenceException"><see cref="Properties"/> is null.</exception>
        public void Save(Stream stream)
        {
            Save(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Saves <see cref="Properties"/> to a specified <paramref name="file"/> using a given <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// </remarks>
        /// <param name="file">The file to save to.</param>
        /// <param name="encoding">The encoding to use when saving.</param>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is null.</exception>
        /// <exception cref="NullReferenceException"><see cref="Properties"/> is null.</exception>
        public void Save(string file, Encoding encoding)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            
            if (Properties == null)
                throw new NullReferenceException(nameof(Properties));

            using (var stream = File.Open(file, FileMode.Create, FileAccess.Write))
                Save(stream, encoding);
        }

        /// <summary>
        /// Saves <see cref="Properties"/> to a specified <paramref name="stream"/> using a given <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// </remarks>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="encoding">The encoding to use when saving.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="NullReferenceException"><see cref="Properties"/> is null.</exception>
        public void Save(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            
            if (Properties == null)
                throw new NullReferenceException(nameof(Properties));
            
            using (var writer = new StreamWriter(stream, encoding))
            {
                foreach (var property in Properties)
                {
                    var value = property.Value ?? string.Empty;

                    if (value.Length == 0 || char.IsWhiteSpace(value[0]))
                        writer.WriteLine($"{property.Key} ~{value}");
                    else
                        writer.WriteLine($"{property.Key} = {value}");
                }
            }
        }
        
        /// <summary>
        /// Tries to save <see cref="Properties"/> to a specified <paramref name="file"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// If <paramref name="file"/> or <see cref="Properties"/> is null, then this will return false.
        /// <br />
        /// The <paramref name="file"/> is saved using UTF-8; use <see cref="Save(string,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The file to save to.</param>
        /// <returns>Returns true if the <paramref name="file"/> was successfully saved to; false if not.</returns>
        public bool TrySave(string file)
        {
            return TrySave(file, Encoding.UTF8);
        }

        /// <summary>
        /// Tries to save <see cref="Properties"/> to a specified <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// If <paramref name="stream"/> or <see cref="Properties"/> is null, then this will return false.
        /// <br />
        /// The <paramref name="stream"/> is saved using UTF-8; use <see cref="Save(Stream,Encoding)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to save to.</param>
        /// <returns>Returns true if the <paramref name="stream"/> was successfully saved to; false if not.</returns>
        public bool TrySave(Stream stream)
        {
            return TrySave(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Tries to save <see cref="Properties"/> to a specified <paramref name="file"/> using a given <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// If <paramref name="file"/> or <see cref="Properties"/> is null, then this will return false.
        /// </remarks>
        /// <param name="file">The file to save to.</param>
        /// <param name="encoding">The encoding to use when saving.</param>
        /// <returns>Returns true if the <paramref name="file"/> was successfully saved to; false if not.</returns>
        public bool TrySave(string file, Encoding encoding)
        {
            if (file == null || Properties == null)
                return false;

            using (var stream = File.Open(file, FileMode.Create, FileAccess.Write))
                return TrySave(stream, encoding);
        }

        /// <summary>
        /// Tries to save <see cref="Properties"/> to a specified <paramref name="stream"/> using a given <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// If a property with a null name is encountered, then it will be saved with an empty name.
        /// <br />
        /// If <paramref name="stream"/> or <see cref="Properties"/> is null, then this will return false.
        /// </remarks>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="encoding">The encoding to use when saving.</param>
        /// <returns>Returns true if the <paramref name="stream"/> was successfully saved to; false if not.</returns>
        public bool TrySave(Stream stream, Encoding encoding)
        {
            if (stream == null || Properties == null)
                return false;

            try
            {
                using (var writer = new StreamWriter(stream, encoding))
                {
                    foreach (var property in Properties)
                    {
                        var value = property.Value ?? string.Empty;

                        if (value.Length == 0 || char.IsWhiteSpace(value[0]))
                            writer.WriteLine($"{property.Key} ~{value}");
                        else
                            writer.WriteLine($"{property.Key} = {value}");
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Copies <see cref="Properties"/> to an <paramref name="array"/>, starting at index 0.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="array"/> is too small to fit all of the properties.</exception>
        public void CopyTo(KeyValuePair<string, string>[] array)
        {
            CopyTo(array, 0);
        }
        
        /// <summary>
        /// Copies <see cref="Properties"/> to an <paramref name="array"/>,
        /// starting at a specified <paramref name="index"/>.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="index">The index of the array to start copying to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="array"/> is too small to fit all of the properties.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Either <paramref name="index"/> is larger than the length of <paramref name="array"/>, less than zero,
        /// or leaves no space for all of the properties to be copied into the <paramref name="array"/>.
        /// </exception>
        public void CopyTo(KeyValuePair<string, string>[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            var length = array.Length;
            var propertyCount = Properties.Count;

            if (propertyCount > length)
                throw new ArgumentException(nameof(array));

            if (index > length || index < 0 || index + propertyCount > length)
                throw new ArgumentOutOfRangeException(nameof(index));

            foreach (var property in Properties)
                array[index++] = property;
        }

        /// <summary>
        /// Get an enumerator that iterates through the <see cref="Properties"/>.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the <see cref="Properties"/>.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets a property's value.
        /// </summary>
        /// <remarks>
        /// If the provided property doesn't exist, it will be created.
        /// <br />
        /// It may be better to use <see cref="GetProperty(string)"/> and <see cref="SetProperty(string,string)"/>
        /// directly as they return a bool to help with error-handling.
        /// </remarks>
        /// <param name="name">The property's name.</param>
        /// <returns>
        /// Returns the property's value if it exists; or returns null if the property doesn't exist or if the given <paramref name="name"/> is null.
        /// </returns>
        public string this[string name]
        {
            get => GetProperty(name);
            set => SetProperty(name, value);
        }
    }
}
