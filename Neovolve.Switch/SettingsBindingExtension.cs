namespace Neovolve.Switch
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// The <see cref="SettingsBindingExtension"/>
    ///   class is used to provide binding accessibility to the application settings.
    /// </summary>
    public class SettingsBindingExtension : Binding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBindingExtension"/> class.
        /// </summary>
        public SettingsBindingExtension()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBindingExtension"/> class.
        /// </summary>
        /// <param name="path">
        /// The initial <see cref="P:System.Windows.Data.Binding.Path"/> for the binding.
        /// </param>
        public SettingsBindingExtension(String path)
            : base(path)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            Source = Properties.Settings.Default;
            Mode = BindingMode.TwoWay;
        }
    }
}