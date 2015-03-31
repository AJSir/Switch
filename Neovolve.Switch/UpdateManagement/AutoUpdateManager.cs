namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="AutoUpdateManager"/>
    ///   class is used to manage the schedule for checking application updates.
    /// </summary>
    [Export(typeof(IApplicationNotification))]
    public class AutoUpdateManager : IApplicationNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoUpdateManager"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="updater">
        /// The updater.
        /// </param>
        [ImportingConstructor]
        public AutoUpdateManager(AutoUpdateSettings settings, AutoUpdater updater)
        {
            Contract.Requires<ArgumentNullException>(settings != null, "The settings value is null.");
            Contract.Requires<ArgumentNullException>(updater != null, "The updater value is null.");

            Settings = settings;
            Updater = updater;
        }

        /// <summary>
        /// Called when the application is closing.
        /// </summary>
        public void OnClosing()
        {
        }

        /// <summary>
        /// Called when a failure has occured.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public void OnFailure(Exception ex)
        {
        }

        /// <summary>
        /// Called when the application is starting.
        /// </summary>
        public void OnStarting()
        {
            if (Settings.Enabled == false)
            {
                IsCompleted = true;

                return;
            }

            if (Settings.CheckFrequency > UpdateFrequency.OnStart)
            {
                if (Settings.CheckFrequency == UpdateFrequency.EachDay && Settings.LastChecked.AddHours(24) > DateTime.UtcNow)
                {
                    IsCompleted = true;

                    return;
                }

                if (Settings.CheckFrequency == UpdateFrequency.EachWeek && Settings.LastChecked.AddDays(7) > DateTime.UtcNow)
                {
                    IsCompleted = true;

                    return;
                }

                if (Settings.CheckFrequency == UpdateFrequency.EachMonth && Settings.LastChecked.AddMonths(1) > DateTime.UtcNow)
                {
                    IsCompleted = true;

                    return;
                }
            }

            // This needs to run on a different thread to allow the UI to show
            Thread versionCheckThread = new Thread(CheckForNewVersion);

            versionCheckThread.Start();
        }

        /// <summary>
        /// Checks for new version.
        /// </summary>
        private void CheckForNewVersion()
        {
            Updater.Execute();

            IsCompleted = true;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is completed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCompleted
        {
            get;
            private set;
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
        /// Gets or sets the updater.
        /// </summary>
        /// <value>
        /// The updater.
        /// </value>
        private AutoUpdater Updater
        {
            get;
            set;
        }
    }
}