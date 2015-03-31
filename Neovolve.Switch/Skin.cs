namespace Neovolve.Switch
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Windows;
    using Neovolve.Switch.Properties;
    using Neovolve.Switch.Skinning;

    /// <summary>
    /// The <see cref="Skin"/>
    ///   class is used to describe a skin.
    /// </summary>
    public class Skin : DependencyObject
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Definition"/> property.
        /// </summary>
        public static readonly DependencyProperty DefinitionProperty = DependencyProperty.Register("Definition", typeof(SkinDefinition), typeof(Skin));

        /// <summary>
        /// Defines the dependency property for the <see cref="DisplayName"/> property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(String), typeof(Skin));

        /// <summary>
        /// Defines the dependency property for the <see cref="FilePath"/> property.
        /// </summary>
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(String), typeof(Skin));

        /// <summary>
        /// Initializes a new instance of the <see cref="Skin"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public Skin(String filePath)
        {
            Contract.Requires<ArgumentNullException>(
                String.IsNullOrWhiteSpace(filePath) == false, "The filePath value is null, empty or only contains whitespace characters");
            Contract.Requires<ArgumentException>(File.Exists(filePath), "The filePath does not exist.");

            DisplayName = Path.GetFileName(filePath);
            FilePath = filePath;

            Load(filePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skin"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public Skin(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null, "The stream value is null.");

            DisplayName = Resources.SkinDefinition_Loading;

            Load(stream);
        }

        /// <summary>
        /// Loads the specified file path.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        private void Load(String filePath)
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Load(stream);
            }
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private void Load(Stream stream)
        {
            SkinDefinition skin = SkinParser.Load(stream, true);

            DisplayName = skin.Description.Name;
            Definition = skin;
        }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public SkinDefinition Definition
        {
            get
            {
                return (SkinDefinition)GetValue(DefinitionProperty);
            }

            set
            {
                SetValue(DefinitionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public String DisplayName
        {
            get
            {
                return (String)GetValue(DisplayNameProperty);
            }

            set
            {
                SetValue(DisplayNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the FilePath.
        /// </summary>
        /// <value>
        /// The FilePath.
        /// </value>
        public String FilePath
        {
            get
            {
                return (String)GetValue(FilePathProperty);
            }

            set
            {
                SetValue(FilePathProperty, value);
            }
        }
    }
}