namespace Neovolve.Switch.Extensibility.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// The <see cref="ServiceManager"/>
    ///   class is used to resolve services for the application.
    /// </summary>
    public static class ServiceManager
    {
        /// <summary>
        /// Stores the composition container.
        /// </summary>
        private static readonly CompositionContainer _container = InitializeContainer();

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to obtain.
        /// </typeparam>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T GetService<T>()
        {
            return _container.GetExportedValueOrDefault<T>();
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <typeparam name="T">
        /// The type of service to obtain.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        public static IEnumerable<T> GetServices<T>()
        {
            return _container.GetExportedValues<T>();
        }

        /// <summary>
        /// Releases the service.
        /// </summary>
        /// <param name="service">
        /// The service instance.
        /// </param>
        public static void ReleaseService(Object service)
        {
            IDisposable disposableService = service as IDisposable;

            if (disposableService == null)
            {
                return;
            }

            disposableService.Dispose();
        }

        /// <summary>
        /// Appends the specified base path.
        /// </summary>
        /// <param name="basePath">
        /// The base path.
        /// </param>
        /// <param name="pathToAppend">
        /// The path to append.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String Append(this String basePath, String pathToAppend)
        {
            if (String.IsNullOrWhiteSpace(basePath))
            {
                return basePath;
            }

            return Path.Combine(basePath, pathToAppend);
        }

        /// <summary>
        /// Evaluates the directory catalog.
        /// </summary>
        /// <param name="aggregateCatalog">
        /// The aggregate catalog.
        /// </param>
        /// <param name="directoryPath">
        /// The directory path.
        /// </param>
        /// <param name="searchPattern">
        /// The search pattern.
        /// </param>
        private static void EvaluateDirectoryCatalog(AggregateCatalog aggregateCatalog, String directoryPath, String searchPattern)
        {
            if (String.IsNullOrWhiteSpace(directoryPath) == false && Directory.Exists(directoryPath))
            {
                DirectoryCatalog directoryCatalog = new DirectoryCatalog(directoryPath, searchPattern);

                aggregateCatalog.Catalogs.Add(directoryCatalog);
            }
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        /// <returns>
        /// A <see cref="CompositionContainer"/> instance.
        /// </returns>
        private static CompositionContainer InitializeContainer()
        {
            const String Plugins = "Plugins";
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AggregateCatalog aggregateCatalog = new AggregateCatalog();
            String domainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String applicationPluginsDirectory = Path.Combine(domainBaseDirectory, Plugins);
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            String userPluginsDirectory =
                Environment.ExpandEnvironmentVariables("%APPDATA%").Append(versionInfo.CompanyName).Append(versionInfo.ProductName).Append(Plugins);

            EvaluateDirectoryCatalog(aggregateCatalog, domainBaseDirectory, "*.exe");
            EvaluateDirectoryCatalog(aggregateCatalog, domainBaseDirectory, "*.dll");
            EvaluateDirectoryCatalog(aggregateCatalog, applicationPluginsDirectory, "*.dll");
            EvaluateDirectoryCatalog(aggregateCatalog, userPluginsDirectory, "*.dll");

            CompositionContainer container = new CompositionContainer(aggregateCatalog);

            return container;
        }
    }
}