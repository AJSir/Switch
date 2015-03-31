namespace Neovolve.Switch.Extensibility.UnitTests.ColorManagement
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Switch.Extensibility.ColorManagement;

    /// <summary>
    /// The <see cref="WindowsColorConverterTests"/>
    ///   class is used to test the <see cref="WindowsColorConverter"/> class.
    /// </summary>
    [TestClass]
    public class WindowsColorConverterTests
    {
        /// <summary>
        /// Runs test for can convert black HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertBlackHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 0, 
                          Saturation = 0, 
                          Luminance = 0
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(0, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(0, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(0, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert black RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertBlackRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 0, 
                          Green = 0, 
                          Blue = 0
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(0, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(0, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(0, actual.Luminance, "Luminance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert blue HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertBlueHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 160, 
                          Saturation = 240, 
                          Luminance = 120
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(0, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(0, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(255, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert blue RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertBlueRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 0, 
                          Green = 0, 
                          Blue = 255
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(160, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(240, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(120, actual.Luminance, "Luminance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert expected HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertExpectedHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 56, 
                          Saturation = 144, 
                          Luminance = 105
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(125, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(178, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(45, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert expected RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertExpectedRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 125, 
                          Green = 179, 
                          Blue = 45
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(56, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(144, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(105, actual.Luminance, "Luminance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert green HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertGreenHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 80, 
                          Saturation = 240, 
                          Luminance = 120
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(0, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(255, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(0, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert green RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertGreenRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 0, 
                          Green = 255, 
                          Blue = 0
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(80, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(240, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(120, actual.Luminance, "Luminance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert red HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertRedHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 0, 
                          Saturation = 240, 
                          Luminance = 120
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(255, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(0, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(0, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert red RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertRedRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 255, 
                          Green = 0, 
                          Blue = 0
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(0, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(240, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(120, actual.Luminance, "Luminance returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert white HSL values to RGB values.
        /// </summary>
        [TestMethod]
        public void CanConvertWhiteHslValuesToRgbValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Hsl hsl = new Hsl
                      {
                          Hue = 0, 
                          Saturation = 0, 
                          Luminance = 240
                      };

            target.AssignHsl(hsl);

            Rgb actual = target.RgbColor;

            Assert.AreEqual(255, actual.Red, "Red returned an incorrect value");
            Assert.AreEqual(255, actual.Green, "Green returned an incorrect value");
            Assert.AreEqual(255, actual.Blue, "Blue returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can convert white RGB values to HSL values.
        /// </summary>
        [TestMethod]
        public void CanConvertWhiteRgbValuesToHslValuesTest()
        {
            IHslScale scale = new WindowsHslScale();
            WindowsColorConverter target = new WindowsColorConverter(scale);
            Rgb rgb = new Rgb
                      {
                          Red = 255, 
                          Green = 255, 
                          Blue = 255
                      };

            target.AssignRgb(rgb);

            Hsl actual = target.HslColor;

            Assert.AreEqual(0, actual.Hue, "Hue returned an incorrect value");
            Assert.AreEqual(0, actual.Saturation, "Saturation returned an incorrect value");
            Assert.AreEqual(240, actual.Luminance, "Luminance returned an incorrect value");
        }
    }
}