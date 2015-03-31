namespace Neovolve.Switch.Extensibility.UnitTests.Services
{
    using System;
    using System.ComponentModel.Composition;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="DisposableApplicationNotification"/>
    ///   class is used to provide a disposable test implementation of the <see cref="IApplicationNotification"/> interface.
    /// </summary>
    [Export(typeof(IApplicationNotification))]
    public class DisposableApplicationNotification : IApplicationNotification, IDisposable
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
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
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: Dispose managed resources here
            }

            // TODO: Dispose unmanaged resources here
            Disposed = true;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DisposableApplicationNotification"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public Boolean Disposed
        {
            get;
            private set;
        }
    }
}