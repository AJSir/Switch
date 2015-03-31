namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The <see cref="IniFile"/>
    ///   class is used to read and write INI formatted files.
    /// </summary>
    internal class IniFile
    {
        /// <summary>
        /// Stores the data loaded for the ini file.
        /// </summary>
        private readonly Dictionary<String, Dictionary<String, String>> _iniData = new Dictionary<String, Dictionary<String, String>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        private IniFile(String content)
        {
            LoadFromData(content);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class. 
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        private IniFile(Stream stream)
        {
            String contents;

            using (StreamReader reader = new StreamReader(stream))
            {
                contents = reader.ReadToEnd();
            }

            LoadFromData(contents);
        }

        /// <summary>
        /// Loads the specified stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// A <see cref="IniFile"/> instance.
        /// </returns>
        public static IniFile Load(Stream stream)
        {
            return new IniFile(stream);
        }

        /// <summary>
        /// Loads the specified data.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// A <see cref="IniFile"/> instance.
        /// </returns>
        public static IniFile Load(String data)
        {
            return new IniFile(data);
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddValue(String sectionName, String key, String value)
        {
            PopulateIniValue(sectionName, key, value);
        }

        /// <summary>
        /// Builds the current definition and returns its content.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public String BuildContent()
        {
            StringBuilder builder = new StringBuilder();

            foreach (String key in _iniData.Keys)
            {
                builder.AppendLine('[' + key + ']');

                Dictionary<String, String> sectionValues = _iniData[key];

                foreach (KeyValuePair<String, String> sectionValue in sectionValues)
                {
                    builder.AppendLine(sectionValue.Key + '=' + sectionValue.Value);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary&lt;TKey,TValue&gt;"/> instance.
        /// </returns>
        public Dictionary<String, String> GetSection(String sectionName)
        {
            if (_iniData.ContainsKey(sectionName) == false)
            {
                return null;
            }

            return _iniData[sectionName];
        }

        /// <summary>
        /// Determines whether [is section name] [the specified data].
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is section name] [the specified data]; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean IsSectionName(String data)
        {
            if (data.StartsWith("[", StringComparison.OrdinalIgnoreCase) == false)
            {
                return false;
            }

            if (data.EndsWith("]", StringComparison.OrdinalIgnoreCase) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the specified contents.
        /// </summary>
        /// <param name="contents">
        /// The contents.
        /// </param>
        private void LoadFromData(String contents)
        {
            _iniData.Clear();

            if (String.IsNullOrWhiteSpace(contents))
            {
                return;
            }

            String currentSection = String.Empty;

            using (StringReader reader = new StringReader(contents))
            {
                String line = reader.ReadLine();

                while (line != null)
                {
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        line = reader.ReadLine();

                        continue;
                    }

                    String data;
                    Int32 commentIndex = line.IndexOf(';');

                    if (commentIndex >= 0)
                    {
                        data = line.Substring(0, commentIndex).Trim();
                    }
                    else
                    {
                        data = line.Trim();
                    }

                    if (IsSectionName(data))
                    {
                        currentSection = data.Substring(1, data.Length - 2);
                    }
                    else if (String.IsNullOrWhiteSpace(currentSection))
                    {
                        // This is some line data that exists before the first section
                        // This will be ignored
                        continue;
                    }
                    else
                    {
                        Char[] delimeters = new[]
                                            {
                                                '='
                                            };
                        String[] parts = data.Split(delimeters);

                        String key = parts[0].Trim();
                        String value = String.Empty;

                        if (parts.Length > 1)
                        {
                            value = parts[1].Trim();
                        }

                        PopulateIniValue(currentSection, key, value);
                    }

                    line = reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Populates the ini value.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void PopulateIniValue(String sectionName, String key, String value)
        {
            Dictionary<String, String> section;

            if (_iniData.ContainsKey(sectionName))
            {
                section = _iniData[sectionName];
            }
            else
            {
                section = new Dictionary<String, String>();

                _iniData.Add(sectionName, section);
            }

            section[key] = value;
        }

        /// <summary>
        /// Gets the section names.
        /// </summary>
        public IEnumerable<String> SectionNames
        {
            get
            {
                return _iniData.Keys;
            }
        }
    }
}