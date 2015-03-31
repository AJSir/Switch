namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// The <see cref="PackageInstallerContracts"/>
    ///   class is used to define the code contracts for the <see cref="IPackageInstaller"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IPackageInstaller))]
    internal abstract class PackageInstallerContracts : IPackageInstaller
    {
        /// <summary>
        /// Installs the package.
        /// </summary>
        /// <param name="package">
        /// The package.
        /// </param>
        /// <param name="packageData">
        /// The package data.
        /// </param>
        public void InstallPackage(PackageDescription package, Stream packageData)
        {
            Contract.Requires<ArgumentNullException>(package != null, "The package value is null.");
            Contract.Requires<ArgumentNullException>(packageData != null, "The packageData value is null.");
        }
    }
}