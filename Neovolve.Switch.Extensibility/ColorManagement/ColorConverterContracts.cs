namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ColorConverterContracts"/>
    ///   class is used to provide the code contracts for the <see cref="IColorConverter"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IColorConverter))]
    internal abstract class ColorConverterContracts : IColorConverter
    {
        /// <summary>
        /// The assign hex.
        /// </summary>
        /// <param name="hexColor">
        /// The hex color.
        /// </param>
        public void AssignHex(String hexColor)
        {
            Contract.Requires<ArgumentNullException>(
                String.IsNullOrWhiteSpace(hexColor) == false, "The hexColor value is null, empty or only contains whitespace characters");
        }

        /// <summary>
        /// The assign hsl.
        /// </summary>
        /// <param name="hslColor">
        /// The hsl color.
        /// </param>
        public void AssignHsl(Hsl hslColor)
        {
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Hue >= 0, "The hslColor.Hue value is less than zero");
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Hue <= HslScaleDescription.HueMax, "The hslColor.Hue value is greater than HslScaleDescription.HueMax");
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Saturation >= 0, "The hslColor.Saturation value is less than zero");
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Saturation <= HslScaleDescription.SaturationMax, "The hslColor.Saturation value is greater than HslScaleDescription.SaturationMax");
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Luminance >= 0, "The hslColor.Luminance value is less than zero");
            Contract.Requires<ArgumentOutOfRangeException>(hslColor.Luminance <= HslScaleDescription.LuminanceMax, "The hslColor.Luminance value is greater than HslScaleDescription.LuminanceMax");
        }

        /// <summary>
        /// The assign ole.
        /// </summary>
        /// <param name="oleColor">
        /// The ole color.
        /// </param>
        public void AssignOle(Int32 oleColor)
        {
            Contract.Requires<ArgumentOutOfRangeException>(oleColor >= 0, "The oleColor value is less than zero");
        }

        /// <summary>
        /// The assign rgb.
        /// </summary>
        /// <param name="rgbColor">
        /// The rgb color.
        /// </param>
        public void AssignRgb(Rgb rgbColor)
        {
            Contract.Requires<ArgumentOutOfRangeException>(rgbColor.Red >= 0, "The rgbColor.Red value is less than zero");
            Contract.Requires<ArgumentOutOfRangeException>(rgbColor.Green >= 0, "The rgbColor.Green value is less than zero");
            Contract.Requires<ArgumentOutOfRangeException>(rgbColor.Blue >= 0, "The rgbColor.Blue value is less than zero");
        }

        /// <summary>
        /// Gets the color of the hex.
        /// </summary>
        /// <value>
        /// The color of the hex.
        /// </value>
        public String HexColor
        {
            get
            {
                Contract.Ensures(
                    String.IsNullOrWhiteSpace(Contract.Result<String>()) == false, 
                    "The HexColor property must return a value that is not null, empty or only white space");

                return null;
            }
        }

        /// <summary>
        /// Gets the color of the HSL.
        /// </summary>
        /// <value>
        /// The color of the HSL.
        /// </value>
        public Hsl HslColor
        {
            get
            {
                Contract.Ensures(
                    Contract.Result<Hsl>().Hue >= 0, "The HslColor property must return a Hue value that is greater than or equal to zero");
                Contract.Ensures(
                    Contract.Result<Hsl>().Saturation >= 0, 
                    "The HslColor property must return a Saturation value that is greater than or equal to zero");
                Contract.Ensures(
                    Contract.Result<Hsl>().Luminance >= 0, "The HslColor property must return a Luminance value that is greater than or equal to zero");

                return default(Hsl);
            }
        }

        /// <summary>
        /// Gets the HSL scale description.
        /// </summary>
        public IHslScale HslScaleDescription
        {
            get
            {
                Contract.Ensures(Contract.Result<IHslScale>() != null, "The HslScaleDescription property must return a value");

                return null;
            }
        }

        /// <summary>
        /// Gets the color of the OLE.
        /// </summary>
        /// <value>
        /// The color of the OLE.
        /// </value>
        public Int32 OleColor
        {
            get
            {
                Contract.Ensures(Contract.Result<Int32>() >= 0, "The OleColor property must return a value that is greater than or equal to zero");

                return 0;
            }
        }

        /// <summary>
        /// Gets the color of the RGB.
        /// </summary>
        /// <value>
        /// The color of the RGB.
        /// </value>
        public Rgb RgbColor
        {
            get
            {
                Contract.Ensures(
                    Contract.Result<Rgb>().Red >= 0, "The RgbColor property must return a Red value that is greater than or equal to zero");
                Contract.Ensures(
                    Contract.Result<Rgb>().Blue >= 0, "The RgbColor property must return a Blue value that is greater than or equal to zero");
                Contract.Ensures(
                    Contract.Result<Rgb>().Green >= 0, "The RgbColor property must return a Green value that is greater than or equal to zero");

                return default(Rgb);
            }
        }
    }
}