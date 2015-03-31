namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="IColorConverter"/>
    ///   interface is used to convert between RGB, HSL, HEX and OLE color systems.
    /// </summary>
    [ContractClass(typeof(ColorConverterContracts))]
    public interface IColorConverter
    {
        /// <summary>
        /// The assign hex.
        /// </summary>
        /// <param name="hexColor">
        /// The hex color.
        /// </param>
        void AssignHex(String hexColor);

        /// <summary>
        /// The assign hsl.
        /// </summary>
        /// <param name="hslColor">
        /// The hsl color.
        /// </param>
        void AssignHsl(Hsl hslColor);

        /// <summary>
        /// The assign ole.
        /// </summary>
        /// <param name="oleColor">
        /// The ole color.
        /// </param>
        void AssignOle(Int32 oleColor);

        /// <summary>
        /// The assign rgb.
        /// </summary>
        /// <param name="rgbColor">
        /// The rgb color.
        /// </param>
        void AssignRgb(Rgb rgbColor);

        /// <summary>
        /// Gets the color of the hex.
        /// </summary>
        /// <value>
        /// The color of the hex.
        /// </value>
        String HexColor
        {
            get;
        }

        /// <summary>
        /// Gets the color of the HSL.
        /// </summary>
        /// <value>
        /// The color of the HSL.
        /// </value>
        Hsl HslColor
        {
            get;
        }

        /// <summary>
        /// Gets the HSL scale description.
        /// </summary>
        IHslScale HslScaleDescription
        {
            get;
        }

        /// <summary>
        /// Gets the color of the OLE.
        /// </summary>
        /// <value>
        /// The color of the OLE.
        /// </value>
        Int32 OleColor
        {
            get;
        }

        /// <summary>
        /// Gets the color of the RGB.
        /// </summary>
        /// <value>
        /// The color of the RGB.
        /// </value>
        Rgb RgbColor
        {
            get;
        }
    }
}