namespace Neovolve.Switch
{
    using System;
    using System.Diagnostics;
    using System.Windows.Data;

    /// <summary>
    /// The <see cref="VersionBindingExtension"/>
    ///   class is used to provide binding accessibility to the application version information.
    /// </summary>
    public class VersionBindingExtension : Binding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionBindingExtension"/> class.
        /// </summary>
        public VersionBindingExtension()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionBindingExtension"/> class.
        /// </summary>
        /// <param name="path">The initial <see cref="P:System.Windows.Data.Binding.Path"/> for the binding.</param>
        public VersionBindingExtension(String path)
            : base(path)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(GetType().Assembly.Location);

            Source = versionInfo;
            Mode = BindingMode.OneTime;
        }
    }
}