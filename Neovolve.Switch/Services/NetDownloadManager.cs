namespace Neovolve.Switch.Services
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Net;
    using Neovolve.Switch.Extensibility.ResourceManagement;

    /// <summary>
    /// The <see cref="NetDownloadManager"/>
    ///   class is used to download a resource from the available network.
    /// </summary>
    [Export(typeof(IDownloadManager))]
    public class NetDownloadManager : IDownloadManager
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
        public Stream Download(Uri address)
        {
            Byte[] downloadData;

            using (WebClient client = new WebClient())
            {
                downloadData = client.DownloadData(address);
            }

            return new MemoryStream(downloadData);
        }
    }
}