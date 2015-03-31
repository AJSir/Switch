namespace Neovolve.Switch.Services
{
    using System;
    using System.ComponentModel.Composition;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="ConfigurationSaver"/>
    ///   class is used to ensure that the application configuration is saved when it closes.
    /// </summary>
    [Export(typeof(IApplicationNotification))]
    internal class ConfigurationSaver : IApplicationNotification
    {
        /// <summary>
        /// Called when the application is closing.
        /// </summary>
        public void OnClosing()
        {
            Properties.Settings.Default.Save();
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
        }
    }
}