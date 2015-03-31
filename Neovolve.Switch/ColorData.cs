namespace Neovolve.Switch
{
    using System;
    using System.Drawing;
    using System.Windows;
    using Neovolve.Switch.Extensibility.ColorManagement;
    using Color = System.Windows.Media.Color;

    /// <summary>
    /// The <see cref="ColorData"/>
    ///   class is used to describe a color in RGB, HSL, Hex and OLE formats.
    /// </summary>
    public class ColorData : DependencyObject
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Blue"/> property.
        /// </summary>
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(Byte), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="CurrentColor"/> property.
        /// </summary>
        public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register("CurrentColor", typeof(Color), typeof(ColorData));

        /// <summary>
        /// The green property.
        /// </summary>
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(Byte), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Hex"/> property.
        /// </summary>
        public static readonly DependencyProperty HexProperty = DependencyProperty.Register("Hex", typeof(String), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Hue"/> property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(Int16), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Luminance"/> property.
        /// </summary>
        public static readonly DependencyProperty LuminanceProperty = DependencyProperty.Register("Luminance", typeof(Int16), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Ole"/> property.
        /// </summary>
        public static readonly DependencyProperty OleProperty = DependencyProperty.Register("Ole", typeof(Int32), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Red"/> property.
        /// </summary>
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(Byte), typeof(ColorData));

        /// <summary>
        /// Defines the dependency property for the <see cref="Saturation"/> property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(Int16), typeof(ColorData));

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorData"/> class.
        /// </summary>
        /// <param name="converter">
        /// The converter.
        /// </param>
        public ColorData(IColorConverter converter)
        {
            IgnoreNotifications = false;
            Converter = converter;

            UpdateColorValues();
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.DependencyObject"/> has been updated. The specific dependency property that changed is reported in the event data.
        /// </summary>
        /// <param name="e">
        /// Event data that will contain the dependency property identifier of interest, the property metadata for the type, and old and new values.
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IgnoreNotifications)
            {
                return;
            }

            IgnoreNotifications = true;

            try
            {
                if (e.Property.Name == RedProperty.Name)
                {
                    SetRgb((Byte)e.NewValue, Green, Blue);
                }
                else if (e.Property.Name == GreenProperty.Name)
                {
                    SetRgb(Red, (Byte)e.NewValue, Blue);
                }
                else if (e.Property.Name == BlueProperty.Name)
                {
                    SetRgb(Red, Green, (Byte)e.NewValue);
                }
                else if (e.Property.Name == HueProperty.Name)
                {
                    Int16 newValue = (Int16)e.NewValue;

                    newValue = EnsureSafeValue(newValue, Converter.HslScaleDescription.HueMax);

                    SetHsl(newValue, Saturation, Luminance);
                }
                else if (e.Property.Name == SaturationProperty.Name)
                {
                    Int16 newValue = (Int16)e.NewValue;

                    newValue = EnsureSafeValue(newValue, Converter.HslScaleDescription.SaturationMax);

                    SetHsl(Hue, newValue, Luminance);
                }
                else if (e.Property.Name == LuminanceProperty.Name)
                {
                    Int16 newValue = (Int16)e.NewValue;

                    newValue = EnsureSafeValue(newValue, Converter.HslScaleDescription.LuminanceMax);

                    SetHsl(Hue, Saturation, newValue);
                }
                else if (e.Property.Name == HexProperty.Name)
                {
                    String newValue = (String)e.NewValue;

                    if (String.IsNullOrWhiteSpace(newValue))
                    {
                        return;
                    }

                    try
                    {
                        String valueToConvert = newValue;

                        if (valueToConvert.StartsWith("#", StringComparison.OrdinalIgnoreCase) == false)
                        {
                            valueToConvert = "#" + valueToConvert;
                        }

                        // Check if this color can be converted
                        ColorTranslator.FromHtml(valueToConvert);

                        if (newValue.Length == 6 && newValue.StartsWith("#", StringComparison.OrdinalIgnoreCase) == false)
                        {
                            newValue = "#" + newValue;
                        }

                        SetHex(newValue);
                    }
                    catch (Exception)
                    {
                        // We don't want this event notification to get progated
                        // We need to assume that the user is still filling out information in the bound textbox
                        return;
                    }
                }
                else if (e.Property.Name == OleProperty.Name)
                {
                    Int32 newValue = (Int32)e.NewValue;

                    if (newValue < 0)
                    {
                        newValue = 0;
                    }

                    SetOle(newValue);
                }
                else
                {
                    SetColor((Color)e.NewValue);
                }

                base.OnPropertyChanged(e);
            }
            finally
            {
                IgnoreNotifications = false;
            }
        }

        /// <summary>
        /// Ensures the safe value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// A <see cref="Int16"/> instance.
        /// </returns>
        private static Int16 EnsureSafeValue(Int16 value, Int16 max)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        private void SetColor(Color newColor)
        {
            SetRgb(newColor.R, newColor.G, newColor.B);
        }

        /// <summary>
        /// Sets the hex.
        /// </summary>
        /// <param name="hex">
        /// The hex.
        /// </param>
        private void SetHex(String hex)
        {
            Converter.AssignHex(hex);

            UpdateColorValues();
        }

        /// <summary>
        /// Sets the HSL.
        /// </summary>
        /// <param name="hue">
        /// The hue.
        /// </param>
        /// <param name="saturation">
        /// The saturation.
        /// </param>
        /// <param name="luminance">
        /// The luminance.
        /// </param>
        private void SetHsl(Int16 hue, Int16 saturation, Int16 luminance)
        {
            Hsl hsl = new Hsl
                      {
                          Hue = hue, 
                          Saturation = saturation, 
                          Luminance = luminance
                      };

            Converter.AssignHsl(hsl);

            UpdateColorValues();
        }

        /// <summary>
        /// Sets the OLE.
        /// </summary>
        /// <param name="ole">
        /// The OLE.
        /// </param>
        private void SetOle(Int32 ole)
        {
            Converter.AssignOle(ole);

            UpdateColorValues();
        }

        /// <summary>
        /// Sets the red, green and blue values.
        /// </summary>
        /// <param name="red">
        /// The red.
        /// </param>
        /// <param name="green">
        /// The green.
        /// </param>
        /// <param name="blue">
        /// The blue.
        /// </param>
        private void SetRgb(Byte red, Byte green, Byte blue)
        {
            Rgb rgb = new Rgb
                      {
                          Red = red, 
                          Green = green, 
                          Blue = blue
                      };

            Converter.AssignRgb(rgb);

            UpdateColorValues();
        }

        /// <summary>
        /// Updates the color values.
        /// </summary>
        private void UpdateColorValues()
        {
            SetValue(HexProperty, Converter.HexColor);
            SetValue(OleProperty, Converter.OleColor);

            Rgb currentRgb = Converter.RgbColor;
            Color newColor = new Color
                             {
                                 A = Byte.MaxValue, 
                                 R = currentRgb.Red, 
                                 G = currentRgb.Green, 
                                 B = currentRgb.Blue
                             };

            SetValue(CurrentColorProperty, newColor);
            SetValue(RedProperty, currentRgb.Red);
            SetValue(GreenProperty, currentRgb.Green);
            SetValue(BlueProperty, currentRgb.Blue);

            Hsl currentHsl = Converter.HslColor;

            SetValue(HueProperty, currentHsl.Hue);
            SetValue(SaturationProperty, currentHsl.Saturation);
            SetValue(LuminanceProperty, currentHsl.Luminance);
        }

        /// <summary>
        /// Gets or sets the Blue.
        /// </summary>
        /// <value>
        /// The Blue.
        /// </value>
        public Byte Blue
        {
            get
            {
                return (Byte)GetValue(BlueProperty);
            }

            set
            {
                SetValue(BlueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        public IColorConverter Converter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CurrentColor.
        /// </summary>
        /// <value>
        /// The CurrentColor.
        /// </value>
        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }

            set
            {
                SetValue(CurrentColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Green.
        /// </summary>
        /// <value>
        /// The Green.
        /// </value>
        public Byte Green
        {
            get
            {
                return (Byte)GetValue(GreenProperty);
            }

            set
            {
                SetValue(GreenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Hex.
        /// </summary>
        /// <value>
        /// The Hex.
        /// </value>
        public String Hex
        {
            get
            {
                return (String)GetValue(HexProperty);
            }

            set
            {
                SetValue(HexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Hue.
        /// </summary>
        /// <value>
        /// The Hue.
        /// </value>
        public Int16 Hue
        {
            get
            {
                return (Int16)GetValue(HueProperty);
            }

            set
            {
                SetValue(HueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Luminance.
        /// </summary>
        /// <value>
        /// The Luminance.
        /// </value>
        public Int16 Luminance
        {
            get
            {
                return (Int16)GetValue(LuminanceProperty);
            }

            set
            {
                SetValue(LuminanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Ole.
        /// </summary>
        /// <value>
        /// The Ole.
        /// </value>
        public Int32 Ole
        {
            get
            {
                return (Int32)GetValue(OleProperty);
            }

            set
            {
                SetValue(OleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Red.
        /// </summary>
        /// <value>
        /// The Red.
        /// </value>
        public Byte Red
        {
            get
            {
                return (Byte)GetValue(RedProperty);
            }

            set
            {
                SetValue(RedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Saturation.
        /// </summary>
        /// <value>
        /// The Saturation.
        /// </value>
        public Int16 Saturation
        {
            get
            {
                return (Int16)GetValue(SaturationProperty);
            }

            set
            {
                SetValue(SaturationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore property change notifications.
        /// </summary>
        /// <value>
        /// <c>true</c> if ignoring property change notifications; otherwise, <c>false</c>.
        /// </value>
        private Boolean IgnoreNotifications
        {
            get;
            set;
        }
    }
}