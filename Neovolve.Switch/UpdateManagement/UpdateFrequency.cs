namespace Neovolve.Switch.UpdateManagement
{
    /// <summary>
    /// The <see cref="UpdateFrequency"/>
    ///   enum defines the frequency the application updates are checked.
    /// </summary>
    public enum UpdateFrequency
    {
        /// <summary>
        /// The update check occurs each time the application is started.
        /// </summary>
        OnStart = 0, 

        /// <summary>
        /// The update check occurs once a day.
        /// </summary>
        EachDay, 

        /// <summary>
        /// The update check occurs once a week.
        /// </summary>
        EachWeek, 

        /// <summary>
        /// The update check occurs once a month.
        /// </summary>
        EachMonth
    }
}