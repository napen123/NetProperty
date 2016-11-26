using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NetProperty
{
    public class PropertyFile
    {
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        public PropertyFile()
        {
        }

        public PropertyFile(string file)
        {
            Load(file);
        }

        public void SetProperty(string name, string value)
        {
            Properties[name] = value;
        }

        public string GetProperty(string name)
        {
            return (from property in Properties where property.Key == name select property.Value).FirstOrDefault();
        }
        
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

        public string this[string name]
        {
            get { return GetProperty(name); }
            set { SetProperty(name, value); }
        }
    }
}
