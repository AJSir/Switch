// -----------------------------------------------------------------------
// <copyright file="WindowsHslScale.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;

    /// <summary>
    /// The <see cref="WindowsHslScale"/>
    ///   class is used to define the HSL scale commonly used in Windows applications.
    /// </summary>
    public class WindowsHslScale : IHslScale
    {
        /// <summary>
        /// Gets the hue description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        public String HueDescription
        {
            get
            {
                return "0 - 240";
            }
        }

        /// <summary>
        /// Gets the maximum hue value supported by this instance.
        /// </summary>
        public Int16 HueMax
        {
            get
            {
                return 240;
            }
        }

        /// <summary>
        /// Gets luminance description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        public String LuminanceDescription
        {
            get
            {
                return "0 - 240";
            }
        }

        /// <summary>
        /// Gets the maximum luminance value supported by this instance.
        /// </summary>
        public Int16 LuminanceMax
        {
            get
            {
                return 240;
            }
        }

        /// <summary>
        /// Gets the saturation description.
        /// </summary>
        /// <remarks>
        /// The description of this value should identify the type of value (numeric, percentage) and its range.
        ///   For example, <c>0 - 100%</c> or <c>0 - 255</c>.
        /// </remarks>
        public String SaturationDescription
        {
            get
            {
                return "0 - 240";
            }
        }

        /// <summary>
        /// Gets the maximum saturation value supported by this instance.
        /// </summary>
        public Int16 SaturationMax
        {
            get
            {
                return 240;
            }
        }
    }
}