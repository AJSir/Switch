namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;

    /// <summary>
    /// The <see cref="Hsl"/>
    ///   struct is used to describe a color using its hue, saturation and luminance components with its non-scaled values.
    /// </summary>
    internal struct CalculatedHsl
    {
        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value>
        /// The hue.
        /// </value>
        public Decimal Hue
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
        public Decimal Luminance
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
        public Decimal Saturation
        {
            get;
            set;
        }
    }
}