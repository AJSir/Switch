namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;

    /// <summary>
    /// The <see cref="Rgb"/>
    ///   struct is used to describe a color using its red, green and blue components.
    /// </summary>
    public struct Rgb
    {
        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value>
        /// The blue.
        /// </value>
        public Byte Blue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value>
        /// The green.
        /// </value>
        public Byte Green
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the red.
        /// </summary>
        /// <value>
        /// The red.
        /// </value>
        public Byte Red
        {
            get;
            set;
        }
    }
}