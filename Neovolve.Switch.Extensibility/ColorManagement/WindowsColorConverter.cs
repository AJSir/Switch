namespace Neovolve.Switch.Extensibility.ColorManagement
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="WindowsColorConverter"/>
    ///   class is used to manage color conversions as commonly expected for windows.
    /// </summary>
    public class WindowsColorConverter : BaseColorConverter
    {
        /// <summary>
        /// Stores the hsl scale.
        /// </summary>
        private readonly IHslScale _scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsColorConverter"/> class.
        /// </summary>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public WindowsColorConverter(IHslScale scale)
        {
            Contract.Requires<ArgumentNullException>(scale != null, "The scale value is null.");

            _scale = scale;
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
        protected override Hsl HslFromRgb(Rgb rgbColor, IHslScale hslScaleDescription)
        {
            Decimal red = rgbColor.Red / 255M; // RGB from 0 to 255
            Decimal green = rgbColor.Green / 255M;
            Decimal blue = rgbColor.Blue / 255M;

            Decimal min = Math.Min(red, Math.Min(green, blue)); // Min. value of RGB
            Decimal max = Math.Max(red, Math.Max(green, blue)); // Max. value of RGB
            Decimal delta = max - min; // Delta RGB value

            CalculatedHsl hsl = new CalculatedHsl
                                {
                                    Luminance = (max + min) / 2M
                                };

            if (delta == 0)
            {
                // This is a gray, no chroma...
                hsl.Hue = 0; // HSL results from 0 to 1
                hsl.Saturation = 0;
            }
            else
            {
                // Chromatic data...
                if (hsl.Luminance < 0.5M)
                {
                    hsl.Saturation = delta / (max + min);
                }
                else
                {
                    hsl.Saturation = delta / (2M - max - min);
                }

                Decimal deltaRed = (((max - red) / 6M) + (delta / 2M)) / delta;
                Decimal deltaGreen = (((max - green) / 6M) + (delta / 2M)) / delta;
                Decimal deltaBlue = (((max - blue) / 6M) + (delta / 2M)) / delta;

                if (red == max)
                {
                    hsl.Hue = deltaBlue - deltaGreen;
                }
                else
                {
                    if (green == max)
                    {
                        hsl.Hue = (1M / 3M) + deltaRed - deltaBlue;
                    }
                    else if (blue == max)
                    {
                        hsl.Hue = (2M / 3M) + deltaGreen - deltaRed;
                    }
                }

                if (hsl.Hue < 0)
                {
                    hsl.Hue += 1M;
                }

                if (hsl.Hue > 1)
                {
                    hsl.Hue -= 1M;
                }
            }

            Hsl response = new Hsl
                           {
                               Hue = Convert.ToInt16(hsl.Hue * hslScaleDescription.HueMax), 
                               Saturation = Convert.ToInt16(hsl.Saturation * hslScaleDescription.SaturationMax), 
                               Luminance = Convert.ToInt16(hsl.Luminance * hslScaleDescription.LuminanceMax)
                           };

            return response;
        }

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
        protected override Rgb RgbFromHsl(Hsl hslColor, IHslScale hslScaleDescription)
        {
            Rgb rgb = new Rgb();
            CalculatedHsl calculatedHsl = new CalculatedHsl
                                          {
                                              Hue = hslColor.Hue / Convert.ToDecimal(hslScaleDescription.HueMax), 
                                              Saturation = hslColor.Saturation / Convert.ToDecimal(hslScaleDescription.SaturationMax), 
                                              Luminance = hslColor.Luminance / Convert.ToDecimal(hslScaleDescription.LuminanceMax)
                                          };

            if (calculatedHsl.Saturation == 0)
            {
                rgb.Red = Convert.ToByte(calculatedHsl.Luminance * 255);
                rgb.Green = rgb.Red;
                rgb.Blue = rgb.Red;
            }
            else
            {
                Decimal max;

                if (calculatedHsl.Luminance < 0.5M)
                {
                    max = calculatedHsl.Luminance * (1 + calculatedHsl.Saturation);
                }
                else
                {
                    max = (calculatedHsl.Luminance + calculatedHsl.Saturation) - (calculatedHsl.Saturation * calculatedHsl.Luminance);
                }

                Decimal min = (2M * calculatedHsl.Luminance) - max;

                rgb.Red = Convert.ToByte(255 * CalculateRgbPart(min, max, calculatedHsl.Hue + (1M / 3M)));
                rgb.Green = Convert.ToByte(255 * CalculateRgbPart(min, max, calculatedHsl.Hue));
                rgb.Blue = Convert.ToByte(255 * CalculateRgbPart(min, max, calculatedHsl.Hue - (1M / 3M)));
            }

            return rgb;
        }

        /// <summary>
        /// Calculates the RGB part.
        /// </summary>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="hueShift">
        /// The hue shift.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal"/> instance.
        /// </returns>
        private static Decimal CalculateRgbPart(Decimal min, Decimal max, Decimal hueShift)
        {
            if (hueShift < 0M)
            {
                hueShift += 1M;
            }

            if (hueShift > 1M)
            {
                hueShift -= 1M;
            }

            if ((6M * hueShift) < 1M)
            {
                return min + ((max - min) * 6M * hueShift);
            }

            if ((2M * hueShift) < 1M)
            {
                return max;
            }

            if ((3M * hueShift) < 2M)
            {
                return min + ((max - min) * ((2M / 3M) - hueShift) * 6M);
            }

            return min;
        }

        /// <summary>
        /// Gets the HSL scale description.
        /// </summary>
        public override IHslScale HslScaleDescription
        {
            get
            {
                return _scale;
            }
        }
    }
}