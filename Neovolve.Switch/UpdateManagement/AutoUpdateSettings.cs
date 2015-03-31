namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// The <see cref="AutoUpdateSettings"/>
    ///   class is used to manage the settings for automatic application updates.
    /// </summary>
    [Export]
    public class AutoUpdateSettings
    {
        /// <summary>
        /// Gets or sets the check frequency.
        /// </summary>
        /// <value>
        /// The check frequency.
        /// </value>
        public UpdateFrequency CheckFrequency
        {
            get
            {
                return Properties.Settings.Default.UpdateFrequency;
            }

            set
            {
                Properties.Settings.Default.UpdateFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AutoUpdateSettings"/> is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public Boolean Enabled
        {
            get
            {
                return Properties.Settings.Default.AutoUpdatesEnabled;
            }

            set
            {
                Properties.Settings.Default.AutoUpdatesEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the last checked.
        /// </summary>
        /// <value>
        /// The last checked.
        /// </value>
        public DateTime LastChecked
        {
            get
            {
                return Properties.Settings.Default.LastChecked;
            }

            set
            {
                Properties.Settings.Default.LastChecked = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the package.
        /// </summary>
        /// <value>
        /// The type of the package.
        /// </value>
        public PackageType PackageType
        {
            get
            {
                return Properties.Settings.Default.UpdatePackageType;
            }

            set
            {
                Properties.Settings.Default.UpdatePackageType = value;
            }
        }

        /// <summary>
        /// Gets the version metadata address.
        /// </summary>
        public Uri VersionMetadataAddress
        {
            get
            {
                return Properties.Settings.Default.VersionCatalogAddress;
            }
        }
    }
}