namespace Neovolve.Switch.UpdateManagement
{
    using System;

    /// <summary>
    /// The <see cref="PackageDescription"/>
    ///   class describes an application package.
    /// </summary>
    public class PackageDescription
    {
        /// <summary>
        /// Gets or sets the package address.
        /// </summary>
        /// <value>
        /// The package address.
        /// </value>
        public String PackageAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the package.
        /// </summary>
        /// <value>
        /// The type of the package.
        /// </value>
        public PackageType PackageType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the package version.
        /// </summary>
        /// <value>
        /// The package version.
        /// </value>
        public PackageVersion PackageVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the release notes address.
        /// </summary>
        /// <value>
        /// The release notes address.
        /// </value>
        public String ReleaseNotesAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the released.
        /// </summary>
        /// <value>
        /// The released.
        /// </value>
        public DateTime Released
        {
            get;
            set;
        }
    }
}