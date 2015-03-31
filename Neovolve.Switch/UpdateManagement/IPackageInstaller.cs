namespace Neovolve.Switch.UpdateManagement
{
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// The <see cref="IPackageInstaller"/>
    ///   interface is used to define the methods for installing a package.
    /// </summary>
    [ContractClass(typeof(PackageInstallerContracts))]
    public interface IPackageInstaller
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
        void InstallPackage(PackageDescription package, Stream packageData);
    }
}