namespace Neovolve.Switch.UpdateManagement
{
    /// <summary>
    /// The <see cref="PackageType"/>
    ///   enum defines the release types of an application.
    /// </summary>
    public enum PackageType
    {
        /// <summary>
        /// The application is a release version.
        /// </summary>
        Release = 0, 

        /// <summary>
        /// The application is a beta version.
        /// </summary>
        Beta, 

        /// <summary>
        /// The application is an alpha version.
        /// </summary>
        Alpha
    }
}