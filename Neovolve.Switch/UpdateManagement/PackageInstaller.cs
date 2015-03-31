namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// The <see cref="PackageInstaller"/>
    ///   class is used to install a package.
    /// </summary>
    [Export(typeof(IPackageInstaller))]
    public class PackageInstaller : IPackageInstaller
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
            Uri packageAddress = new Uri(package.PackageAddress);
            String tempDirectory = Path.GetTempPath();
            String packagePath = packageAddress.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            String packageFilename = Path.GetFileName(packagePath);
            String tempPath = Path.Combine(tempDirectory, packageFilename);

            WritePackageToDisk(tempPath, packageData);

            Process.Start(tempPath);

            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }

        /// <summary>
        /// Writes the package to disk.
        /// </summary>
        /// <param name="tempPath">
        /// The temp path.
        /// </param>
        /// <param name="packageStream">
        /// The package stream.
        /// </param>
        private static void WritePackageToDisk(String tempPath, Stream packageStream)
        {
            using (FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                const Int32 BufferSize = 8192;

                while (packageStream.Position < packageStream.Length)
                {
                    Int32 bufferLength = BufferSize;

                    if (bufferLength + packageStream.Position > packageStream.Length)
                    {
                        bufferLength = Convert.ToInt32(packageStream.Length - packageStream.Position);
                    }

                    Byte[] buffer = new Byte[bufferLength];

                    packageStream.Read(buffer, 0, bufferLength);
                    fileStream.Write(buffer, 0, bufferLength);
                }
            }
        }
    }
}