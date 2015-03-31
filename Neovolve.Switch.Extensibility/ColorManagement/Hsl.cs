namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;

    /// <summary>
    /// The <see cref="Hsl"/>
    ///   struct is used to describe a color using its hue, saturation and luminance components.
    /// </summary>
    public struct Hsl
    {
        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value>
        /// The hue.
        /// </value>
        public Int16 Hue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the luminance.
        /// </summary>
        /// <value>
        /// The luminance.
        /// </value>
        public Int16 Luminance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the saturation.
        /// </summary>
        /// <value>
        /// The saturation.
        /// </value>
        public Int16 Saturation
        {
            get;
            set;
        }
    }
}