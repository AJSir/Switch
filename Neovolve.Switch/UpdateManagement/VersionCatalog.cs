namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// The <see cref="VersionCatalog"/>
    ///   class is used to contain a set of <see cref="PackageDescription"/> instances.
    /// </summary>
    public class VersionCatalog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCatalog"/> class.
        /// </summary>
        public VersionCatalog()
        {
            Packages = new Collection<PackageDescription>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCatalog"/> class.
        /// </summary>
        /// <param name="packages">
        /// The packages.
        /// </param>
        public VersionCatalog(IEnumerable<PackageDescription> packages)
        {
            Contract.Requires<ArgumentNullException>(packages != null, "The packages value is null.");

            Packages = new Collection<PackageDescription>(packages.ToList());
        }

        /// <summary>
        /// Gets or sets the packages.
        /// </summary>
        /// <value>
        /// The packages.
        /// </value>
        public Collection<PackageDescription> Packages
        {
            get;
            set;
        }
    }
}