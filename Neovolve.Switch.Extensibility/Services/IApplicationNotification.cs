namespace Neovolve.Switch.Extensibility.Services
{
    using System;

    /// <summary>
    /// The <see cref="IApplicationNotification"/>
    ///   interface is used to define the application notification support.
    /// </summary>
    public interface IApplicationNotification
    {
        /// <summary>
        /// Called when the application is closing.
        /// </summary>
        void OnClosing();

        /// <summary>
        /// Called when a failure has occured.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        void OnFailure(Exception ex);

        /// <summary>
        /// Called when the application is starting.
        /// </summary>
        void OnStarting();
    }
}