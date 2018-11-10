using System;
using System.IO;
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
        /// Initializes a new instance of PropertyFile with an initial starting <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The properties to initialize with.</param>
        /// <see cref="Dictionary{TKey,TValue}(int)"/>
        public PropertyFile(int capacity)
        {
            Properties = new Dictionary<string, string>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading properties from a <paramref name="file" />.
        /// </summary>
        /// <remarks>
        /// The <paramref name="file"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(string,Encoding,bool)"/> to specify another.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(string file, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(file, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading as many properties from a <paramref name="file" /> as possible.
        /// </summary>
        /// <remarks>
        /// The <paramref name="file"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(string,Encoding,out bool,bool)"/> to specify another.
        /// </remarks>
        /// <param name="file">The file to load from.</param>
        /// <param name="result">The result of the load: true if all properties were successfully loaded; false if some were incorrectly declared.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(string file, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(file, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading as many properties from a <paramref name="file" /> as possible,
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="result">The result of the load: true if all properties were successfully loaded; false if some were incorrectly declared.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(string file, Encoding encoding, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(file, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading properties from a <paramref name="file" />
        /// using a specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(string file, Encoding encoding, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(file, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="stream"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(Stream,Encoding,bool)"/> to specify another.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(Stream stream, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(stream, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading from a <paramref name="stream"/>
        /// using a specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="stream"/>.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(Stream stream, Encoding encoding, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            Load(stream, encoding, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading as many properties from a <paramref name="stream" /> as possible.
        /// </summary>
        /// <remarks>
        /// The <paramref name="stream"/> will be loaded as UTF-8.
        /// Use <see cref="PropertyFile(Stream,Encoding,out bool,bool)"/> to specify another.
        /// </remarks>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="result">The result of the load: true if all properties were successfully loaded; false if some were incorrectly declared.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(Stream stream, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(stream, Encoding.UTF8, false, treatEmptyAsNull);
        }

        /// <summary>
        /// Initializes a new instance of PropertyFile by loading as many properties from a <paramref name="stream" /> as possible,
        /// using the provided <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="stream"/>.</param>
        /// <param name="result">The result of the load: true if all properties were successfully loaded; false if some were incorrectly declared.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null when loading.</param>
        public PropertyFile(Stream stream, Encoding encoding, out bool result, bool treatEmptyAsNull = false)
        {
            Properties = new Dictionary<string, string>();

            result = TryLoad(stream, encoding, false, treatEmptyAsNull);
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
        /// <returns>If <paramref name="name"/> is null, returns false; otherwise, returns true.</returns>
        public bool SetProperty(string name, string value)
        {
            if (name == null)
                return false;

            Properties[name] = value;

            return true;
        }

        /// <summary>
        /// Get a property's value.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <returns>If <paramref name="name"/> is null or the property doesn't exist, returns null; otherwise, returns the value of the property.</returns>
        public string GetProperty(string name)
        {
            if (name == null || !Properties.TryGetValue(name, out var value))
                return null;

            return value;
        }

        /// <summary>
        /// Load a property <paramref name="file"/>. If <paramref name="clearExisting"/> is true,
        /// remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// <br />
        /// The <paramref name="file"/> will be opened as UTF-8; use <see cref="Load(string,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        public void Load(string file, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            Load(file, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Load a property file from a <paramref name="stream"/>. If <paramref name="clearExisting"/> is true,
        /// remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br />
        /// The <paramref name="stream"/> will be opened as UTF-8; use <see cref="Load(Stream,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        public void Load(Stream stream, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            Load(stream, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Load properties from a <paramref name="file"/> using the specified <paramref name="encoding"/>. If <paramref name="clearExisting"/> is true,
        /// remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        public void Load(string file, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                Load(stream, encoding, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Load properties from a <paramref name="stream"/> using the specified <paramref name="encoding"/>. If <paramref name="clearExisting"/> is true,
        /// remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encoding">The encoding to use when opening the file.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        /// <exception cref="InvalidPropertyException">Thrown if a property is declared incorrectly (i.e. missing either a "=" or "~").</exception>
        public void Load(Stream stream, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
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
                        throw new InvalidPropertyException("Expected either \'=\' or \'~\' : " + line);
                }
            }
        }

        /// <summary>
        /// Try to load as many properties from a <paramref name="file"/> as possible.
        /// If <paramref name="clearExisting"/> is true, remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// <br />
        /// The <paramref name="file"/> will be opened as UTF-8; use <see cref="Load(string,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        /// <returns>True if all properties were successfully loaded; false if some were unable to be loaded (i.e. invalid syntax).</returns>
        public bool TryLoad(string file, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            return TryLoad(file, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Try to load as many properties from a <paramref name="stream"/> as possible.
        /// If <paramref name="clearExisting"/> is true, remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br />
        /// The <paramref name="stream"/> will be opened as UTF-8; use <see cref="Load(Stream,Encoding,bool,bool)"/> for alternate encodings.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        /// <returns>True if all properties were successfully loaded; false if some were unable to be loaded (i.e. invalid syntax).</returns>
        public bool TryLoad(Stream stream, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            return TryLoad(stream, Encoding.UTF8, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Try to load as many properties from a <paramref name="file"/> using the specified <paramref name="encoding"/> as possible.
        /// If <paramref name="clearExisting"/> is true, remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="file"/>.
        /// </remarks>
        /// <param name="file">The property file to load.</param>
        /// <param name="encoding">The encoding to use when reading the <paramref name="file"/>.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        /// <returns>True if all properties were successfully loaded; false if some were unable to be loaded (i.e. invalid syntax).</returns>
        public bool TryLoad(string file, Encoding encoding, bool clearExisting = true, bool treatEmptyAsNull = false)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                return TryLoad(stream, encoding, clearExisting, treatEmptyAsNull);
        }

        /// <summary>
        /// Try to load as many properties from a <paramref name="stream"/> using the specified <paramref name="encoding"/> as possible.
        /// If <paramref name="clearExisting"/> is true, remove all existing properties; if false, keep the existing properties.
        /// </summary>
        /// <remarks>
        /// Properties can have their values overriden if redefined in the <paramref name="stream"/>.
        /// <br/>
        /// <see cref="Properties"/> will be initialized if it's null.
        /// </remarks>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encoding">The encoding to use when opening the file.</param>
        /// <param name="clearExisting">Remove existing properties before loading.</param>
        /// <param name="treatEmptyAsNull">If true, empty values will be added as null.</param>
        /// <exception cref="InvalidPropertyException">Thrown if a property is declared incorrectly (i.e. missing either a "=" or "~").</exception>
        /// <returns>True if all properties were successfully loaded; false if some were unable to be loaded (i.e. invalid syntax).</returns>
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

                    if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal))
                        continue;

                    if (trimmed.Contains("="))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else if (trimmed.Contains("~"))
                    {
                        var name = trimmed.Substring(0, trimmed.IndexOf("~", StringComparison.Ordinal)).TrimEnd();
                        var value = trimmed.Substring(trimmed.IndexOf("~", StringComparison.Ordinal) + 1);

                        Properties[name] = treatEmptyAsNull && value.Length == 0 ? null : value;
                    }
                    else
                        ret = false;
                }
            }

            return ret;
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
        /// Save <see cref="Properties"/> to a given <paramref name="file"/> using a specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="file">The file to save to.</param>
        /// <param name="encoding">The encoding to save the <paramref name="file"/> as.</param>
        public void Save(string file, Encoding encoding)
        {
            using (var stream = File.Open(file, FileMode.Create, FileAccess.Write))
                Save(stream, encoding);
        }

        /// <summary>
        /// Save <see cref="Properties"/> to a given <paramref name="stream"/> using a specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="encoding">The encoding to save the <paramref name="stream"/> as.</param>
        public void Save(Stream stream, Encoding encoding)
        {
            using (var writer = new StreamWriter(stream, encoding))
            {
                if (Properties == null || Properties.Count == 0)
                    return;

                foreach (var property in Properties)
                {
                    var value = property.Value ?? "";

                    if (value.Length == 0 || char.IsWhiteSpace(value[0]))
                        writer.WriteLine(property.Key + " ~" + value);
                    else
                        writer.WriteLine(property.Key + " = " + value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a property's value.
        /// </summary>
        /// <remarks>
        /// When setting, if there's a possibility of <paramref name="name"/> being null,
        /// it would be better to use <see cref="SetProperty"/> because it will return false in that case;
        /// this error-case can then be handled better.
        /// </remarks>
        /// <param name="name">The property's name.</param>
        /// <returns>Returns the property's value; if the property doesn't exist or <paramref name="name"/> is null, returns null.</returns>
        public string this[string name]
        {
            get => GetProperty(name);
            set => SetProperty(name, value);
        }
    }
}
