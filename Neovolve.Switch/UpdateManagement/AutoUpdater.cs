namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using Neovolve.Switch.Extensibility.ResourceManagement;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="AutoUpdater"/>
    ///   class is used to provide logic for detecting new application releases 
    ///   and providing the facility to install them.
    /// </summary>
    [Export]
    public class AutoUpdater
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoUpdater"/> class.
        /// </summary>
        /// <param name="downloadManager">
        /// The download manager.
        /// </param>
        /// <param name="logWriter">
        /// The log writer.
        /// </param>
        /// <param name="userNotification">
        /// The user notification.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="packageInstaller">
        /// The package installer.
        /// </param>
        [ImportingConstructor]
        public AutoUpdater(
            IDownloadManager downloadManager, 
            ILogWriter logWriter, 
            IUserNotification userNotification, 
            AutoUpdateSettings settings, 
            IPackageInstaller packageInstaller)
        {
            Contract.Requires<ArgumentNullException>(downloadManager != null, "The downloadManager value is null.");
            Contract.Requires<ArgumentNullException>(logWriter != null, "The logWriter value is null.");
            Contract.Requires<ArgumentNullException>(userNotification != null, "The userNotification value is null.");
            Contract.Requires<ArgumentNullException>(settings != null, "The settings value is null.");
            Contract.Requires<ArgumentNullException>(packageInstaller != null, "The packageInstaller value is null.");

            DownloadManager = downloadManager;
            LogWriter = logWriter;
            UserNotification = userNotification;
            Settings = settings;
            Installer = packageInstaller;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public virtual void Execute()
        {
            try
            {
                VersionCatalog catalog = GetCatalog();

                if (catalog == null)
                {
                    LogWriter.Write(TraceEventType.Warning, "Unable to download version catalog from {0}.", Settings.VersionMetadataAddress);

                    return;
                }

                PackageVersion currentVersion = DetermineCurrentVersion();

                if (currentVersion == null)
                {
                    LogWriter.Write(TraceEventType.Warning, "Unable to determine current application version.");

                    return;
                }

                IOrderedEnumerable<PackageDescription> availablePackages = from x in catalog.Packages
                                                                           where
                                                                               x.PackageType <= Settings.PackageType &&
                                                                               x.PackageVersion > currentVersion
                                                                           orderby x.PackageVersion descending
                                                                           select x;

                if (availablePackages.Any() == false)
                {
                    LogWriter.Write(
                        TraceEventType.Information, 
                        "Version check found that this version is the latest version when searching for package type '{0}'.", 
                        Settings.PackageType);

                    Settings.LastChecked = DateTime.UtcNow;

                    return;
                }

                PackageDescription package = availablePackages.First();

                String packageVersion = package.PackageVersion.ToString();

                if (package.PackageType > PackageType.Release)
                {
                    packageVersion += " (" + package.PackageType + ")";
                }

                if (UserNotification.AskQuestion("Switch " + packageVersion + " is now available. Do you want to install it?") == false)
                {
                    return;
                }

                Uri packageAddress = new Uri(package.PackageAddress, UriKind.RelativeOrAbsolute);
                
                // If the package address is relative, make it absolute against the catalog address
                if (packageAddress.IsAbsoluteUri == false)
                {
                    packageAddress = new Uri(Settings.VersionMetadataAddress, packageAddress);
                }

                using (Stream packageData = DownloadManager.Download(packageAddress))
                {
                    Installer.InstallPackage(package, packageData);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write(
                    TraceEventType.Error, "Failed to identify available application updates from '{0}'.\n\n{1}", Settings.VersionMetadataAddress, ex);
            }
        }

        /// <summary>
        /// Determines the current version.
        /// </summary>
        /// <returns>
        /// A <see cref="Version"/> instance.
        /// </returns>
        private PackageVersion DetermineCurrentVersion()
        {
            String assemblyLocation = GetType().Assembly.Location;

            if (String.IsNullOrWhiteSpace(assemblyLocation))
            {
                LogWriter.Write(TraceEventType.Verbose, "Unable to determine assembly file path. This may be a dynamic assembly.");

                return null;
            }

            if (File.Exists(assemblyLocation) == false)
            {
                LogWriter.Write(TraceEventType.Verbose, "Unable to determine assembly version. Assembly does not exist on disk.");

                return null;
            }

            FileVersionInfo applicationVersion = FileVersionInfo.GetVersionInfo(assemblyLocation);

            return new PackageVersion(
                applicationVersion.ProductMajorPart, 
                applicationVersion.ProductMinorPart, 
                applicationVersion.ProductBuildPart, 
                applicationVersion.ProductPrivatePart);
        }

        /// <summary>
        /// Gets the catalog.
        /// </summary>
        /// <returns>
        /// A <see cref="VersionCatalog"/> instance.
        /// </returns>
        private VersionCatalog GetCatalog()
        {
            using (Stream versionMetadata = DownloadManager.Download(Settings.VersionMetadataAddress))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(VersionCatalog));

                return serializer.Deserialize(versionMetadata) as VersionCatalog;
            }
        }

        /// <summary>
        /// Gets or sets the download manager.
        /// </summary>
        /// <value>
        /// The download manager.
        /// </value>
        private IDownloadManager DownloadManager
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the installer.
        /// </summary>
        /// <value>
        /// The installer.
        /// </value>
        private IPackageInstaller Installer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the log writer.
        /// </summary>
        /// <value>
        /// The log writer.
        /// </value>
        private ILogWriter LogWriter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        private AutoUpdateSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prompt manager.
        /// </summary>
        /// <value>
        /// The prompt manager.
        /// </value>
        private IUserNotification UserNotification
        {
            get;
            set;
        }
    }
}