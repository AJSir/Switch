namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Ionic.Zip;
    using Brush = System.Windows.Media.Brush;
    using Brushes = System.Windows.Media.Brushes;
    using Color = System.Drawing.Color;
    using FontFamily = System.Windows.Media.FontFamily;
    using FontStyle = System.Windows.FontStyle;

    /// <summary>
    /// The <see cref="SkinExtensions"/>
    ///   class is used to provide extension methods for managing skins.
    /// </summary>
    internal static class SkinExtensions
    {
        /// <summary>
        /// Defines the set of types supported by the <see cref="Convert"/> class.
        /// </summary>
        private static readonly List<Type> _supportedConvertTypes = new List<Type>
                                                                    {
                                                                        typeof(Object), 
                                                                        typeof(DBNull), 
                                                                        typeof(Boolean), 
                                                                        typeof(Char), 
                                                                        typeof(SByte), 
                                                                        typeof(Byte), 
                                                                        typeof(Int16), 
                                                                        typeof(UInt16), 
                                                                        typeof(Int32), 
                                                                        typeof(UInt32), 
                                                                        typeof(Int64), 
                                                                        typeof(UInt64), 
                                                                        typeof(Single), 
                                                                        typeof(Double), 
                                                                        typeof(Decimal), 
                                                                        typeof(DateTime), 
                                                                        typeof(String)
                                                                    };

        /// <summary>
        /// Finds the first matching entry.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// A <see cref="ZipEntry"/> instance.
        /// </returns>
        public static ZipEntry FindFirstMatchingEntry(this ZipFile zip, String fileName)
        {
            ZipEntry entry = zip.Entries.FirstOrDefault(x => x.FileName.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                String message = "Skin does not contain the '" + fileName + "' file.";

                throw new SkinLoadException(message);
            }

            return entry;
        }

        /// <summary>
        /// Gets the specified configuration.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value to return.
        /// </typeparam>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Get<T>(this IniFile configuration, String sectionName, String key)
        {
            return Get(configuration, sectionName, key, () => default(T));
        }

        /// <summary>
        /// Gets the specified configuration.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value to return.
        /// </typeparam>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Get<T>(this IniFile configuration, String sectionName, String key, Func<T> defaultValue)
        {
            Dictionary<String, String> section = configuration.GetSection(sectionName);

            if (section == null)
            {
                return defaultValue();
            }

            if (section.ContainsKey(key) == false)
            {
                return defaultValue();
            }

            String configurationValue = section[key];

            if (String.IsNullOrEmpty(configurationValue))
            {
                return defaultValue();
            }

            Type returnType = typeof(T);

            if (returnType.Equals(typeof(String)))
            {
                return (T)(Object)configurationValue;
            }

            if (_supportedConvertTypes.Contains(returnType))
            {
                return (T)Convert.ChangeType(configurationValue, returnType, CultureInfo.CurrentCulture);
            }

            // Get the type converter for the type to return
            TypeConverter converter = TypeDescriptor.GetConverter(returnType);

            if (converter == null)
            {
                return defaultValue();
            }

            // Convert from a string to the type required
            return (T)converter.ConvertFromString(configurationValue);
        }

        /// <summary>
        /// Gets the skin body.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="mouseOffSectionName">
        /// Name of the mouse off section.
        /// </param>
        /// <param name="mouseOverSectionName">
        /// Name of the mouse over section.
        /// </param>
        /// <param name="mouseDownSectionName">
        /// Name of the mouse down section.
        /// </param>
        /// <returns>
        /// A <see cref="SkinButton"/> instance.
        /// </returns>
        public static SkinButton GetSkinBody(
            this ZipFile zip, IniFile configuration, String mouseOffSectionName, String mouseOverSectionName, String mouseDownSectionName)
        {
            Color transparentColour = Color.Empty;
            String transparentValue = configuration.Get<String>(SkinSection.Skin, SkinKey.TransparentColor);

            if (String.IsNullOrWhiteSpace(transparentValue) == false)
            {
                // Convert this value to a color
                ColorConverter converter = new ColorConverter();

                transparentColour = (Color)converter.ConvertFromString(transparentValue);
            }

            SkinImage mouseOff = zip.GetSkinImage(configuration, mouseOffSectionName, true, SkinKey.Image, null, transparentColour);

            SkinButton button = new SkinButton
                                {
                                    MouseDown =
                                        zip.GetSkinImage(configuration, mouseDownSectionName, false, SkinKey.Image, mouseOff, transparentColour),
                                    MouseOff = mouseOff,
                                    MouseOver =
                                        zip.GetSkinImage(configuration, mouseOverSectionName, false, SkinKey.Image, mouseOff, transparentColour)
                                };

            return button;
        }

        /// <summary>
        /// Gets the skin button.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="mouseOffSectionName">
        /// Name of the mouse off section.
        /// </param>
        /// <param name="mouseOverSectionName">
        /// Name of the mouse over section.
        /// </param>
        /// <param name="mouseDownSectionName">
        /// Name of the mouse down section.
        /// </param>
        /// <returns>
        /// A <see cref="SkinButton"/> instance.
        /// </returns>
        public static SkinButton GetSkinButton(
            this ZipFile zip, IniFile configuration, String mouseOffSectionName, String mouseOverSectionName, String mouseDownSectionName)
        {
            SkinImage mouseOff = zip.GetSkinImage(configuration, mouseOffSectionName, false);

            SkinButton button = new SkinButton
                                {
                                    MouseDown = zip.GetSkinImage(configuration, mouseDownSectionName, false, mouseOff),
                                    MouseOff = mouseOff,
                                    MouseOver = zip.GetSkinImage(configuration, mouseOverSectionName, false, mouseOff)
                                };

            return button;
        }

        /// <summary>
        /// Gets the skin image.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c> [is required].
        /// </param>
        /// <param name="imageKey">
        /// The image key.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <param name="transparentColor">
        /// Color of the transparent.
        /// </param>
        /// <returns>
        /// A <see cref="SkinImage"/> instance.
        /// </returns>
        public static SkinImage GetSkinImage(
            this ZipFile zip,
            IniFile configuration,
            String sectionName,
            Boolean isRequired,
            String imageKey,
            SkinImage template,
            Color transparentColor)
        {
            if (String.IsNullOrWhiteSpace(sectionName))
            {
                if (isRequired)
                {
                    String message = "The skin definition does not contain the required section '" + sectionName + ".";

                    throw new SkinLoadException(message);
                }

                return null;
            }

            String fileName = configuration.Get<String>(sectionName, imageKey);
            ImageSource image = null;
            Region region = null;

            if (String.IsNullOrWhiteSpace(fileName))
            {
                if (template != null && template.Source != null)
                {
                    image = template.Source;
                    fileName = template.Name;
                    region = template.Region;
                }
                else if (isRequired)
                {
                    String message = "The skin file does not contain the required image for section '" + sectionName + "' and key '" + imageKey +
                                     "'.";

                    throw new SkinLoadException(message);
                }
            }
            else
            {
                Bitmap bitmap = zip.LoadBitmap(fileName);
                image = bitmap.ToImageSource();

                if (transparentColor != Color.Empty)
                {
                    String regionFileName = Path.GetFileNameWithoutExtension(fileName) + ".rgn";
                    ZipEntry regionFile = zip.Entries.FirstOrDefault(x => x.FileName.EndsWith(regionFileName, StringComparison.OrdinalIgnoreCase));

                    if (regionFile != null)
                    {
                        // Load the region
                        Int32 uncompressedSize = (Int32)regionFile.UncompressedSize;
                        Byte[] regionData;

                        using (MemoryStream stream = new MemoryStream(uncompressedSize))
                        {
                            regionFile.Extract(stream);
                            stream.Position = 0;

                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                regionData = reader.ReadBytes(uncompressedSize);
                            }
                        }

                        using (Region tempRegion = new Region())
                        {
                            RegionData tempData = tempRegion.GetRegionData();

                            if (tempData != null)
                            {
                                tempData.Data = regionData;

                                region = new Region(tempData);
                            }
                        }
                    }
                    else
                    {
                        // Calculate the region
                        region = bitmap.GetRegion(transparentColor);

                        if (String.IsNullOrWhiteSpace(zip.Name) == false
                            && region != null)
                        {
                            // This is a skin on the file system
                            try
                            {
                                // Attempt to save the region to the zip file
                                RegionData regionData = region.GetRegionData();

                                if (regionData != null)
                                {
                                    zip.AddEntry(regionFileName, regionData.Data);
                                    zip.Save();
                                }
                            }
                            catch (IOException)
                            {
                                // Ignore this exception
                            }
                            catch (UnauthorizedAccessException)
                            {
                                // Ignore this exception
                            }
                        }
                    }
                }
            }

            SkinPart part = GetSkinPart(zip, configuration, sectionName, template);

            if (image != null)
            {
                if (part.Height == 0)
                {
                    part.Height = Convert.ToInt32(image.Height);
                }

                if (part.Width == 0)
                {
                    part.Width = Convert.ToInt32(image.Width);
                }
            }

            SkinImage skinImage = new SkinImage(part)
                                  {
                                      Name = fileName,
                                      Source = image
                                  };

            return new SkinImage(skinImage)
                   {
                       Region = region
                   };
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c> [is required].
        /// </param>
        /// <returns>
        /// A <see cref="System.Windows.Media.ImageSource"/> instance.
        /// </returns>
        public static SkinImage GetSkinImage(this ZipFile zip, IniFile configuration, String sectionName, Boolean isRequired)
        {
            return GetSkinImage(zip, configuration, sectionName, isRequired, SkinKey.Image, null, Color.Empty);
        }

        /// <summary>
        /// Gets the skin image.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c> [is required].
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <returns>
        /// A <see cref="SkinImage"/> instance.
        /// </returns>
        public static SkinImage GetSkinImage(this ZipFile zip, IniFile configuration, String sectionName, Boolean isRequired, SkinImage template)
        {
            return GetSkinImage(zip, configuration, sectionName, isRequired, SkinKey.Image, template, Color.Empty);
        }

        /// <summary>
        /// Gets the skin text box.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="mouseOffSectionName">
        /// Name of the mouse off section.
        /// </param>
        /// <param name="mouseOverSectionName">
        /// Name of the mouse over section.
        /// </param>
        /// <returns>
        /// A <see cref="SkinTextBox"/> instance.
        /// </returns>
        public static SkinTextBox GetSkinTextBox(this ZipFile zip, IniFile configuration, String mouseOffSectionName, String mouseOverSectionName)
        {
            SkinTextBoxState mouseOff = zip.GetSkinTextBoxState(configuration, mouseOffSectionName, null);

            SkinTextBox textBox = new SkinTextBox
                                  {
                                      MouseOff = mouseOff,
                                      MouseOver = zip.GetSkinTextBoxState(configuration, mouseOverSectionName, mouseOff)
                                  };

            return textBox;
        }

        /// <summary>
        /// The get skin text box state.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <returns>
        /// A <see cref="SkinTextBoxState"/> instance.
        /// </returns>
        public static SkinTextBoxState GetSkinTextBoxState(this ZipFile zip, IniFile configuration, String sectionName, SkinTextBoxState template)
        {
            SkinPart part = zip.GetSkinPart(configuration, sectionName, template);

            BrushConverter converter = new BrushConverter();
            Brush background;

            if (configuration.HasValue(sectionName, SkinKey.Background))
            {
                String backgroundValue = configuration.Get<String>(sectionName, SkinKey.Background);
                background = (Brush)converter.ConvertFrom(backgroundValue);
            }
            else
            {
                background = configuration.GetTemplatedConfigValue(
                    sectionName, SkinKey.Background, template, () => template.Background, Brushes.White);
            }

            Brush foreground;

            if (configuration.HasValue(sectionName, SkinKey.Foreground))
            {
                String foregroundValue = configuration.Get<String>(sectionName, SkinKey.Foreground);

                foreground = (Brush)converter.ConvertFrom(foregroundValue);
            }
            else
            {
                foreground = configuration.GetTemplatedConfigValue(
                    sectionName, SkinKey.Foreground, template, () => template.Foreground, Brushes.Black);
            }

            FontWeight fontWeight;

            if (configuration.HasValue(sectionName, SkinKey.FontBold))
            {
                Boolean isBold = configuration.Get<Boolean>(sectionName, SkinKey.FontBold);

                if (isBold)
                {
                    fontWeight = FontWeights.Bold;
                }
                else
                {
                    fontWeight = FontWeights.Normal;
                }
            }
            else
            {
                fontWeight = configuration.GetTemplatedConfigValue(
                    sectionName, SkinKey.FontBold, template, () => template.FontWeight, FontWeights.Normal);
            }

            FontStyle fontStyle;

            if (configuration.HasValue(sectionName, SkinKey.FontItalic))
            {
                Boolean isItalic = configuration.Get<Boolean>(sectionName, SkinKey.FontItalic);

                if (isItalic)
                {
                    fontStyle = FontStyles.Italic;
                }
                else
                {
                    fontStyle = FontStyles.Normal;
                }
            }
            else
            {
                fontStyle = configuration.GetTemplatedConfigValue(
                    sectionName, SkinKey.FontItalic, template, () => template.FontStyle, FontStyles.Normal);
            }

            FontFamily fontFamily;

            if (configuration.HasValue(sectionName, SkinKey.FontFamily))
            {
                fontFamily = configuration.Get<FontFamily>(sectionName, SkinKey.FontFamily);
            }
            else
            {
                fontFamily = configuration.GetTemplatedConfigValue(sectionName, SkinKey.FontFamily, template, () => template.FontFamily);
            }

            Double fontSize;
            FontSizeConverter fontSizeConverter = new FontSizeConverter();

            if (configuration.HasValue(sectionName, SkinKey.FontSize))
            {
                String fontSizeValue = configuration.Get(sectionName, SkinKey.FontSize, () => "10") + "pt";

                fontSize = (Double)fontSizeConverter.ConvertFromString(fontSizeValue);
            }
            else
            {
                fontSize = configuration.GetTemplatedConfigValue(
                    sectionName, SkinKey.FontSize, template, () => template.FontSize, (Double)fontSizeConverter.ConvertFromString("10pt"));
            }

            return new SkinTextBoxState(part)
                   {
                       Background = background,
                       Foreground = foreground,
                       FontFamily = fontFamily,
                       FontSize = fontSize,
                       FontWeight = fontWeight,
                       FontStyle = fontStyle
                   };
        }

        /// <summary>
        /// Determines whether the specified configuration has value.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified configuration has value; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasValue(this IniFile configuration, String sectionName, String key)
        {
            Dictionary<String, String> section = configuration.GetSection(sectionName);

            if (section == null)
            {
                return false;
            }

            if (section.ContainsKey(key) == false)
            {
                return false;
            }

            String configurationValue = section[key];

            if (String.IsNullOrEmpty(configurationValue))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the specified definition.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="loadOverviewOnly">
        /// If set to <c>true</c> [load overview only].
        /// </param>
        public static void Load(this SkinDefinition definition, ZipFile zip, IniFile configuration, Boolean loadOverviewOnly)
        {
            definition.Description.Name = configuration.Get<String>(SkinSection.Skin, SkinKey.Name);
            definition.Description.Description = configuration.Get<String>(SkinSection.Skin, SkinKey.Comments);
            definition.Description.Version = configuration.Get<String>(SkinSection.Skin, SkinKey.Version);
            definition.Description.Author.Name = configuration.Get<String>(SkinSection.Skin, SkinKey.Author);
            definition.Description.Author.Email = configuration.Get<String>(SkinSection.Skin, SkinKey.Email);
            definition.Description.Author.Website = configuration.Get<String>(SkinSection.Skin, SkinKey.Url);

            String screenshotName = configuration.Get<String>(SkinSection.Skin, SkinKey.Screenshot);

            if (String.IsNullOrWhiteSpace(screenshotName) == false)
            {
                definition.Description.Screenshot = zip.GetSkinImage(configuration, SkinSection.Skin, false, SkinKey.Screenshot, null, Color.Empty);
            }

            if (loadOverviewOnly)
            {
                return;
            }

            // Additional common properties
            // TransparentColor=#FF00FF
            // ToolTipBackColor=#FFFFE1
            // ToolTipForeColor=#000000
            // ToolTipBalloon=False
            definition.Body = zip.GetSkinBody(configuration, SkinSection.Main, null, null);
            definition.Red = zip.GetSkinTextBox(configuration, SkinSection.Red, SkinSection.RedHover);
            definition.RedMore = zip.GetSkinButton(configuration, SkinSection.RedMoreUp, SkinSection.RedMoreHover, SkinSection.RedMoreDown);
            definition.RedLess = zip.GetSkinButton(configuration, SkinSection.RedLessUp, SkinSection.RedLessHover, SkinSection.RedLessDown);
            definition.Green = zip.GetSkinTextBox(configuration, SkinSection.Green, SkinSection.GreenHover);
            definition.GreenMore = zip.GetSkinButton(configuration, SkinSection.GreenMoreUp, SkinSection.GreenMoreHover, SkinSection.GreenMoreDown);
            definition.GreenLess = zip.GetSkinButton(configuration, SkinSection.GreenLessUp, SkinSection.GreenLessHover, SkinSection.GreenLessDown);
            definition.Blue = zip.GetSkinTextBox(configuration, SkinSection.Blue, SkinSection.BlueHover);
            definition.BlueMore = zip.GetSkinButton(configuration, SkinSection.BlueMoreUp, SkinSection.BlueMoreHover, SkinSection.BlueMoreDown);
            definition.BlueLess = zip.GetSkinButton(configuration, SkinSection.BlueLessUp, SkinSection.BlueLessHover, SkinSection.BlueLessDown);
            definition.Hue = zip.GetSkinTextBox(configuration, SkinSection.Hue, SkinSection.HueHover);
            definition.HueMore = zip.GetSkinButton(configuration, SkinSection.HueMoreUp, SkinSection.HueMoreHover, SkinSection.HueMoreDown);
            definition.HueLess = zip.GetSkinButton(configuration, SkinSection.HueLessUp, SkinSection.HueLessHover, SkinSection.HueLessDown);
            definition.Saturation = zip.GetSkinTextBox(configuration, SkinSection.Saturation, SkinSection.SaturationHover);
            definition.SaturationMore = zip.GetSkinButton(
                configuration, SkinSection.SaturationMoreUp, SkinSection.SaturationMoreHover, SkinSection.SaturationMoreDown);
            definition.SaturationLess = zip.GetSkinButton(
                configuration, SkinSection.SaturationLessUp, SkinSection.SaturationLessHover, SkinSection.SaturationLessDown);
            definition.LuminanceMore = zip.GetSkinButton(
                configuration, SkinSection.LuminanceMoreUp, SkinSection.LuminanceMoreHover, SkinSection.LuminanceMoreDown);
            definition.LuminanceLess = zip.GetSkinButton(
                configuration, SkinSection.LuminanceLessUp, SkinSection.LuminanceLessHover, SkinSection.LuminanceLessDown);
            definition.Luminance = zip.GetSkinTextBox(configuration, SkinSection.Luminance, SkinSection.LuminanceHover);
            definition.Hex = zip.GetSkinTextBox(configuration, SkinSection.Hex, SkinSection.HexHover);
            definition.Ole = zip.GetSkinTextBox(configuration, SkinSection.Ole, SkinSection.OleHover);
            definition.CurrentColor = zip.GetSkinPart(configuration, SkinSection.CurrentColor, null);
            definition.Pick = zip.GetSkinButton(configuration, SkinSection.PickUp, SkinSection.PickHover, SkinSection.PickDown);
            definition.Close = zip.GetSkinButton(configuration, SkinSection.CloseUp, SkinSection.CloseHover, SkinSection.CloseDown);
            definition.Options = zip.GetSkinButton(configuration, SkinSection.OptionsUp, SkinSection.OptionsHover, SkinSection.OptionsDown);
            definition.ZoomCapture = zip.GetSkinPart(configuration, SkinSection.ZoomedScreenshot, null);
            definition.Zoom = zip.GetSkinButton(configuration, SkinSection.ZoomUp, SkinSection.ZoomHover, SkinSection.ZoomDown);
            definition.OnTop = zip.GetSkinCheckBox(
                configuration, SkinSection.TopOn, SkinSection.TopOff, SkinSection.TopOnHover, SkinSection.TopOffHover);
        }

        /// <summary>
        /// Loads the entry as stream.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> instance.
        /// </returns>
        public static Stream LoadEntryAsStream(this ZipFile zip, String fileName)
        {
            ZipEntry entry = zip.FindFirstMatchingEntry(fileName);

            Stream entryData = new MemoryStream();

            entry.Extract(entryData);
            entryData.Position = 0;

            return entryData;
        }

        /// <summary>
        /// Loads the entry as text.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static String LoadEntryAsText(this ZipFile zip, String fileName)
        {
            ZipEntry entry = zip.FindFirstMatchingEntry(fileName);

            using (Stream entryData = new MemoryStream())
            {
                entry.Extract(entryData);
                entryData.Position = 0;

                using (StreamReader reader = new StreamReader(entryData))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Gets the skin check box.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="topOnSectionName">
        /// Name of the top on section.
        /// </param>
        /// <param name="topOffSectionName">
        /// Name of the top off section.
        /// </param>
        /// <param name="topOnHoverSectionName">
        /// Name of the top on hover section.
        /// </param>
        /// <param name="topOffHoverSectionName">
        /// Name of the top off hover section.
        /// </param>
        /// <returns>
        /// A <see cref="SkinCheckBox"/> instance.
        /// </returns>
        private static SkinCheckBox GetSkinCheckBox(
            this ZipFile zip,
            IniFile configuration,
            String topOnSectionName,
            String topOffSectionName,
            String topOnHoverSectionName,
            String topOffHoverSectionName)
        {
            SkinImage checkedImage = zip.GetSkinImage(configuration, topOnSectionName, true);
            SkinImage uncheckedImage = zip.GetSkinImage(configuration, topOffSectionName, true);

            SkinCheckBox checkBox = new SkinCheckBox
                                    {
                                        Checked = checkedImage,
                                        CheckedMouseOver = zip.GetSkinImage(configuration, topOnHoverSectionName, false, checkedImage),
                                        Unchecked = uncheckedImage,
                                        UncheckedMouseOver = zip.GetSkinImage(configuration, topOffHoverSectionName, false, uncheckedImage)
                                    };

            return checkBox;
        }

        /// <summary>
        /// Gets the skin part.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <returns>
        /// A <see cref="SkinPart"/> instance.
        /// </returns>
        private static SkinPart GetSkinPart(this ZipFile zip, IniFile configuration, String sectionName, SkinPart template)
        {
            SkinPart part = new SkinPart
                            {
                                Top = GetTemplatedConfigValue(configuration, sectionName, SkinKey.Top, template, () => template.Top),
                                Left = GetTemplatedConfigValue(configuration, sectionName, SkinKey.Left, template, () => template.Left),
                                Height = GetTemplatedConfigValue(configuration, sectionName, SkinKey.Height, template, () => template.Height),
                                Width = GetTemplatedConfigValue(configuration, sectionName, SkinKey.Width, template, () => template.Width),
                                Cursor = zip.LoadCursor(configuration, sectionName)
                            };

            return part;
        }

        /// <summary>
        /// Gets the templated config value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of configuration value.
        /// </typeparam>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <param name="templateValue">
        /// The template value.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        private static T GetTemplatedConfigValue<T>(
            this IniFile configuration, String sectionName, String key, SkinPart template, Func<T> templateValue)
        {
            return GetTemplatedConfigValue(configuration, sectionName, key, template, templateValue, default(T));
        }

        /// <summary>
        /// Gets the templated config value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value to return.
        /// </typeparam>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <param name="templateValue">
        /// The template value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        private static T GetTemplatedConfigValue<T>(
            this IniFile configuration, String sectionName, String key, SkinPart template, Func<T> templateValue, T defaultValue)
        {
            if (configuration.HasValue(sectionName, key))
            {
                return configuration.Get<T>(sectionName, key);
            }

            if (template == null)
            {
                return defaultValue;
            }

            return templateValue();
        }

        /// <summary>
        /// Loads the bitmap.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// A <see cref="System.Windows.Media.ImageSource"/> instance.
        /// </returns>
        private static Bitmap LoadBitmap(this ZipFile file, String fileName)
        {
            using (Stream imageData = file.LoadEntryAsStream(fileName))
            {
                if (imageData == null)
                {
                    return null;
                }

                return new Bitmap(imageData);
            }
        }

        /// <summary>
        /// Loads the cursor.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="sectionName">
        /// Name of the section.
        /// </param>
        /// <returns>
        /// A <see cref="Cursor"/> instance.
        /// </returns>
        private static Cursor LoadCursor(this ZipFile zip, IniFile configuration, String sectionName)
        {
            String fileName = configuration.Get<String>(sectionName, SkinKey.Cursor);

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            using (Stream reader = zip.LoadEntryAsStream(fileName))
            {
                return new Cursor(reader);
            }
        }
    }
}