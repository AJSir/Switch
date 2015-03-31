namespace Neovolve.Switch.Extensibility.ResourceManagement
{
    using System;
    using System.IO;

    /// <summary>
    /// The <see cref="IDownloadManager"/>
    ///   interface is used to define the operations for downloading artifacts from a specified address.
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// Downloads the specified address.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> instance.
        /// </returns>
        Stream Download(Uri address);
    }
}