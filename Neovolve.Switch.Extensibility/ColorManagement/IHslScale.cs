namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;

    /// <summary>
    /// The <see cref="IHslScale"/>
    /// interface defines the scaling information used to describe a <see cref="Hsl"/> color.
    /// </summary>
    public interface IHslScale
    {
        /// <summary>
        /// Gets the hue description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        String HueDescription
        {
            get;
        }

        /// <summary>
        /// Gets the maximum hue value supported by this instance.
        /// </summary>
        Int16 HueMax
        {
            get;
        }

        /// <summary>
        /// Gets luminance description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        String LuminanceDescription
        {
            get;
        }

        /// <summary>
        /// Gets the maximum luminance value supported by this instance.
        /// </summary>
        Int16 LuminanceMax
        {
            get;
        }

        /// <summary>
        /// Gets the saturation description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        String SaturationDescription
        {
            get;
        }

        /// <summary>
        /// Gets the maximum saturation value supported by this instance.
        /// </summary>
        Int16 SaturationMax
        {
            get;
        }
    }
}