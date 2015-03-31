namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;
    using System.Drawing;

    /// <summary>
    /// The <see cref="BaseColorConverter"/>
    ///   class is used to provide the base conversion algorithms for RGB, OLE and HEX color representations.
    /// </summary>
    public abstract class BaseColorConverter : IColorConverter
    {
        /// <summary>
        /// The assign hex.
        /// </summary>
        /// <param name="hexColor">
        /// The hex color.
        /// </param>
        public void AssignHex(String hexColor)
        {
            HexColor = hexColor;

            RgbColor = RgbFromHex(hexColor);

            OleColor = OleFromRgb(RgbColor);
            HslColor = HslFromRgb(RgbColor, HslScaleDescription);
        }

        /// <summary>
        /// The assign hsl.
        /// </summary>
        /// <param name="hslColor">
        /// The hsl color.
        /// </param>
        public void AssignHsl(Hsl hslColor)
        {
            HslColor = hslColor;

            RgbColor = RgbFromHsl(hslColor, HslScaleDescription);

            HexColor = HexFromRgb(RgbColor);
            OleColor = OleFromRgb(RgbColor);
        }

        /// <summary>
        /// The assign ole.
        /// </summary>
        /// <param name="oleColor">
        /// The ole color.
        /// </param>
        public void AssignOle(Int32 oleColor)
        {
            OleColor = oleColor;

            RgbColor = RgbFromOle(oleColor);

            HexColor = HexFromRgb(RgbColor);
            HslColor = HslFromRgb(RgbColor, HslScaleDescription);
        }

        /// <summary>
        /// The assign rgb.
        /// </summary>
        /// <param name="rgbColor">
        /// The rgb color.
        /// </param>
        public void AssignRgb(Rgb rgbColor)
        {
            RgbColor = rgbColor;

            HexColor = HexFromRgb(rgbColor);
            HslColor = HslFromRgb(rgbColor, HslScaleDescription);
            OleColor = OleFromRgb(rgbColor);
        }

        /// <summary>
        /// HSLs from RGB.
        /// </summary>
        /// <param name="rgbColor">
        /// Color of the RGB.
        /// </param>
        /// <param name="hslScaleDescription">
        /// The HSL scale description.
        /// </param>
        /// <returns>
        /// A <see cref="Hsl"/> instance.
        /// </returns>
        protected abstract Hsl HslFromRgb(Rgb rgbColor, IHslScale hslScaleDescription);

        /// <summary>
        /// RGBs from HSL.
        /// </summary>
        /// <param name="hslColor">
        /// Color of the HSL.
        /// </param>
        /// <param name="hslScaleDescription">
        /// The HSL scale description.
        /// </param>
        /// <returns>
        /// A <see cref="Rgb"/> instance.
        /// </returns>
        protected abstract Rgb RgbFromHsl(Hsl hslColor, IHslScale hslScaleDescription);

        /// <summary>
        /// Hexes from RGB.
        /// </summary>
        /// <param name="rgbColor">
        /// Color of the RGB.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String HexFromRgb(Rgb rgbColor)
        {
            Color colorToConvert = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);

            return ColorTranslator.ToHtml(colorToConvert);
        }

        /// <summary>
        /// OLEs from RGB.
        /// </summary>
        /// <param name="rgbColor">
        /// Color of the RGB.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        private static Int32 OleFromRgb(Rgb rgbColor)
        {
            Color colorToConvert = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);

            return ColorTranslator.ToOle(colorToConvert);
        }

        /// <summary>
        /// RGBs from color.
        /// </summary>
        /// <param name="translatedColor">
        /// Color of the translated.
        /// </param>
        /// <returns>
        /// A <see cref="Rgb"/> instance.
        /// </returns>
        private static Rgb RgbFromColor(Color translatedColor)
        {
            return new Rgb
                   {
                       Blue = translatedColor.B, 
                       Green = translatedColor.G, 
                       Red = translatedColor.R
                   };
        }

        /// <summary>
        /// RGBs from hex.
        /// </summary>
        /// <param name="hexColor">
        /// Color of the hex.
        /// </param>
        /// <returns>
        /// A <see cref="Rgb"/> instance.
        /// </returns>
        private static Rgb RgbFromHex(String hexColor)
        {
            if (hexColor.StartsWith("#", StringComparison.OrdinalIgnoreCase) == false)
            {
                hexColor = "#" + hexColor;
            }

            Color translatedColor = ColorTranslator.FromHtml(hexColor);

            return RgbFromColor(translatedColor);
        }

        /// <summary>
        /// RGBs from OLE.
        /// </summary>
        /// <param name="oleColor">
        /// Color of the OLE.
        /// </param>
        /// <returns>
        /// A <see cref="Rgb"/> instance.
        /// </returns>
        private static Rgb RgbFromOle(Int32 oleColor)
        {
            Color translatedColor = ColorTranslator.FromOle(oleColor);

            return RgbFromColor(translatedColor);
        }

        /// <summary>
        /// Gets the color of the hex.
        /// </summary>
        /// <value>
        /// The color of the hex.
        /// </value>
        public String HexColor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the color of the HSL.
        /// </summary>
        /// <value>
        /// The color of the HSL.
        /// </value>
        public Hsl HslColor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the HSL scale description.
        /// </summary>
        public abstract IHslScale HslScaleDescription
        {
            get;
        }

        /// <summary>
        /// Gets the color of the OLE.
        /// </summary>
        /// <value>
        /// The color of the OLE.
        /// </value>
        public Int32 OleColor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the color of the RGB.
        /// </summary>
        /// <value>
        /// The color of the RGB.
        /// </value>
        public Rgb RgbColor
        {
            get;
            private set;
        }
    }
}