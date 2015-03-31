namespace Neovolve.Switch
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.Practices.Unity;
    using Neovolve.Switch.Properties;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="OptionsWindow"/>
    ///   class is used to display an options window.
    /// </summary>
    internal partial class OptionsWindow : Window
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="OriginalSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty OriginalSettingsProperty = DependencyProperty.Register(
            "OriginalSettings", typeof(Settings), typeof(OptionsWindow));

        /// <summary>
        /// Defines the dependency property for the <see cref="UpdatedSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty UpdatedSettingsProperty = DependencyProperty.Register(
            "UpdatedSettings", typeof(Settings), typeof(OptionsWindow));

        /// <summary>
        /// Defines the dependency property for the <see cref="Skins"/> property.
        /// </summary>
        public static readonly DependencyProperty SkinsProperty = DependencyProperty.Register("Skins", typeof(SkinSet), typeof(OptionsWindow));

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWindow"/> class.
        /// </summary>
        public OptionsWindow()
        {
            Skins = DomainContainer.Current.Resolve<SkinSet>();

            Skins.SetDispatcher(Dispatcher);
            
            TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(PopulateSkins, CancellationToken.None, TaskCreationOptions.None, uiContext);

            InitializeComponent();
        }

        /// <summary>
        /// Populates the skins.
        /// </summary>
        private void PopulateSkins()
        {
            Skins.LoadSkins();

            AvailableSkins.SelectedValue = OriginalSettings.CurrentSkinPath;

            if (AvailableSkins.SelectedItem == null)
            {
                AvailableSkins.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Clones the settings.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// A <see cref="Settings"/> instance.
        /// </returns>
        private static Settings CloneSettings(Settings settings)
        {
            Settings clone = new Settings();

            settings.CopySettings(clone);

            return clone;
        }

        /// <summary>
        /// Handles the Click event of the OK control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void OK_Click(Object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Close();
        }

        /// <summary>
        /// Gets or sets the original settings.
        /// </summary>
        /// <value>
        /// The original settings.
        /// </value>
        public Settings OriginalSettings
        {
            get
            {
                return (Settings)GetValue(OriginalSettingsProperty);
            }

            set
            {
                SetValue(OriginalSettingsProperty, value);

                UpdatedSettings = CloneSettings(value);
            }
        }

        /// <summary>
        /// Gets or sets the updated settings.
        /// </summary>
        /// <value>
        /// The updated settings.
        /// </value>
        public Settings UpdatedSettings
        {
            get
            {
                return (Settings)GetValue(UpdatedSettingsProperty);
            }

            set
            {
                SetValue(UpdatedSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the skins.
        /// </summary>
        /// <value>
        /// The skins.
        /// </value>
        public SkinSet Skins
        {
            get
            {
                return (SkinSet)GetValue(SkinsProperty);
            }

            set
            {
                SetValue(SkinsProperty, value);
            }
        }
    }
}