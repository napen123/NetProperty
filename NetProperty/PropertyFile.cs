using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NetProperty
{
    public class PropertyFile<T>
    {
        public string File;
        public Dictionary<string, T> Properties = new Dictionary<string, T>();

        public PropertyFile(string file, bool loadFile = true)
        {
            File = file;

            if(loadFile)
                Load(File);
        }

        public void SetPropertyValue(string name, T value, bool canCreateNewProperty = true)
        {
            if (Properties.ContainsKey(name))
                Properties[name] = value;
            else
            {
                if (canCreateNewProperty)
                    Properties.Add(name, value);
                else
                    throw new Exception("No property exists with that name : " + name);
            }
        }

        public T GetPropertyValue(string property)
        {
            return (from p in Properties where p.Key == property select p.Value).FirstOrDefault();
        }

        public void Load(Func<string, T> converter = null, bool clearProperties = true)
        {
            Load(File, converter, clearProperties);
        }

        public void Load(string file, Func<string, T> converter = null, bool clearProperties = true)
        {
            if (!System.IO.File.Exists(file))
                throw new IOException("Property file does not exist : " + file);
            if (converter == null)
            {
                converter = value =>
                {
                    try
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unexpected error while converting property value : " + e.Message);
                    }
                };
            }
            if (clearProperties)
                Properties.Clear();

            foreach (var line in System.IO.File.ReadLines(file))
            {
                var trimmed = line.Trim();

                if (trimmed.Length == 0 || trimmed.StartsWith("#"))
                    continue;

                if (trimmed.Contains("="))
                {
                    var name = trimmed.Substring(0, trimmed.IndexOf('=')).TrimEnd();
                    var value = trimmed.Substring(trimmed.IndexOf('=') + 1).TrimStart();
                    
                    Properties[name] = converter(value);
                }
                else if (trimmed.Contains("~"))
                {
                    var name = trimmed.Substring(0, trimmed.IndexOf("~", StringComparison.Ordinal)).TrimEnd();
                    var value = trimmed.Substring(trimmed.IndexOf("~", StringComparison.Ordinal) + 1);

                    Properties[name] = converter(value);
                }
                else
                    throw new Exception("Expected either \'=\' or \'~\' : " + line);
            }
        }

        public void Save(Func<T, string> converter = null)
        {
            Save(File, converter);
        }

        public void Save(string file, Func<T, string> converter = null)
        {
            if (converter == null)
                converter = s => s.ToString();

            using (var writer = new StreamWriter(File))
            {
                foreach (var property in Properties)
                {
                    var strValue = converter(property.Value);

                    if (strValue.Length == 0)
                        writer.WriteLine(property.Key + " =");
                    else if (char.IsWhiteSpace(strValue[0]))
                        writer.WriteLine(property.Key + " ~" + strValue);
                    else
                        writer.WriteLine(property.Key + " = " + strValue);
                }
            }
        }

        public T this[string property]
        {
            get { return GetPropertyValue(property); }
            set
            {
                SetPropertyValue(property, value);
            }
        }

        public T this[string property, bool canCreateNewProperty]
        {
            set
            {
                SetPropertyValue(property, value, canCreateNewProperty);
            }
        }
    }

    public class PropertyFile : PropertyFile<string>
    {
        public PropertyFile(string file, bool loadFile = true)
            : base(file, loadFile)
        {
        }
    }
}
