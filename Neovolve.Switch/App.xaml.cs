namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Practices.Unity;
    using Neovolve.Switch.Extensibility.Services;
    using Neovolve.Toolkit.Unity;

    /// <summary>
    /// The <see cref="App"/>
    ///   class is used for loading the application.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnExit(ExitEventArgs e)
        {
            List<IApplicationNotification> applicationNotifications = ServiceManager.GetServices<IApplicationNotification>().ToList();

            applicationNotifications.ForEach(x => SafelyExecutionAction(x.OnClosing));
            applicationNotifications.ForEach(ServiceManager.ReleaseService);

            base.OnExit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += GenericExceptionHandler;
            Current.DispatcherUnhandledException += DispatcherExceptionHandler;

            if (e.Args.Length > 0)
            {
                ProcessSkinFiles(e.Args);
            }

            List<IApplicationNotification> applicationNotifications = ServiceManager.GetServices<IApplicationNotification>().ToList();

            applicationNotifications.ForEach(x => SafelyExecutionAction(x.OnStarting));
            applicationNotifications.ForEach(ServiceManager.ReleaseService);

            base.OnStartup(e);
        }

        /// <summary>
        /// Dispatchers the exception handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.
        /// </param>
        private static void DispatcherExceptionHandler(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogError(e.Exception.ToString());

            e.Handled = true;

            RequestRestart();
        }

        /// <summary>
        /// Generics the exception handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.
        /// </param>
        private static void GenericExceptionHandler(Object sender, UnhandledExceptionEventArgs e)
        {
            LogError(e.ExceptionObject.ToString());

            RequestRestart();
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private static void LogError(String message)
        {
            List<ILogWriter> logWriters = ServiceManager.GetServices<ILogWriter>().ToList();

            logWriters.ForEach(x => x.Write(TraceEventType.Error, message));
            logWriters.ForEach(ServiceManager.ReleaseService);
        }

        /// <summary>
        /// Processes the skin files.
        /// </summary>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        private static void ProcessSkinFiles(IEnumerable<String> arguments)
        {
            SkinSet skins = DomainContainer.Current.Resolve<SkinSet>();
            ISkinStore writableStore = skins.Stores.FirstOrDefault(x => x.IsWritable);

            if (writableStore == null)
            {
                // There is no writable store
                return;
            }

            String storedSkin = String.Empty;

            foreach (String argument in arguments)
            {
                if (File.Exists(argument))
                {
                    String newSkinPath = writableStore.StoreSkin(argument);

                    if (String.IsNullOrWhiteSpace(newSkinPath) == false)
                    {
                        storedSkin = argument;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(storedSkin) == false)
            {
                // We have successfully stored at least one skin
                // Make this skin the currently selected skin
                Neovolve.Switch.Properties.Settings.Default.CurrentSkinPath = storedSkin;
            }
        }

        /// <summary>
        /// Requests the restart.
        /// </summary>
        private static void RequestRestart()
        {
            MessageBoxResult result = MessageBox.Show(
                "Switch has encountered an error and needs to close. Would you like to restart Switch?", 
                "Error", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                System.Windows.Forms.Application.Restart();
            }

            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Safelies the execution action.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        private static void SafelyExecutionAction(Action action)
        {
            try
            {
                action();
            }
            catch (NotImplementedException)
            {
                // Ignore this exception
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }
        }
    }
}