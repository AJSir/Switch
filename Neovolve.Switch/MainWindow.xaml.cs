namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using Microsoft.Practices.Unity;
    using Neovolve.Switch.Extensibility.ColorManagement;
    using Neovolve.Switch.Skinning;
    using Neovolve.Toolkit.Unity;
    using Button = System.Windows.Controls.Button;
    using Color = System.Windows.Media.Color;
    using PixelFormat = System.Drawing.Imaging.PixelFormat;
    using Point = System.Windows.Point;

    /// <summary>
    /// The <see cref="MainWindow"/>
    ///   class is used to display the main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="CurrentColor"/> property.
        /// </summary>
        public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
            "CurrentColor", 
            typeof(ColorData), 
            typeof(MainWindow), 
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Defines the dependency property for the <see cref="ScreenCapture"/> property.
        /// </summary>
        public static readonly DependencyProperty ScreenCaptureProperty = DependencyProperty.Register(
            "ScreenCapture", 
            typeof(ImageSource), 
            typeof(MainWindow), 
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Defines the dependency property for the <see cref="Skin"/> property.
        /// </summary>
        public static readonly DependencyProperty SkinProperty = DependencyProperty.Register("Skin", typeof(SkinDefinition), typeof(MainWindow));

        /// <summary>
        /// Defines the dependency property for the <see cref="ZoomArea"/> property.
        /// </summary>
        public static readonly DependencyProperty ZoomAreaProperty = DependencyProperty.Register("ZoomArea", typeof(Rect), typeof(MainWindow));

        /// <summary>
        /// Stores the last pick position of the mouse.
        /// </summary>
        private Point _lastPickPosition;

        /// <summary>
        /// Stores the last zoom position of the mouse.
        /// </summary>
        private Point _lastZoomPosition;

        /// <summary>
        /// Stores the position that the last capture started.
        /// </summary>
        private Point _startCapturePosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            IColorConverter converter = DomainContainer.Current.Resolve<IColorConverter>();
            String previousColor = Properties.Settings.Default.CurrentColor;

            converter.AssignHex(previousColor);

            CurrentColor = new ColorData(converter);

            LoadSkin(Properties.Settings.Default.CurrentSkinPath);

            InitializeComponent();

            // Create image transform
            CreateImageTransform();

            HookEvents();
        }

        /// <summary>
        /// Bits the BLT.
        /// </summary>
        /// <param name="hDC">
        /// The h DC.
        /// </param>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="nWidth">
        /// Width of the n.
        /// </param>
        /// <param name="nHeight">
        /// Height of the n.
        /// </param>
        /// <param name="hSrcDC">
        /// The h SRC DC.
        /// </param>
        /// <param name="xSrc">
        /// The x SRC.
        /// </param>
        /// <param name="ySrc">
        /// The y SRC.
        /// </param>
        /// <param name="dwRop">
        /// The dw rop.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern Int32 BitBlt(
            IntPtr hDC, Int32 x, Int32 y, Int32 nWidth, Int32 nHeight, IntPtr hSrcDC, Int32 xSrc, Int32 ySrc, Int32 dwRop);

        /// <summary>
        /// Calculates the screen area.
        /// </summary>
        /// <returns>
        /// A <see cref="Rectangle"/> instance.
        /// </returns>
        private static Rectangle CalculateScreenArea()
        {
            List<Screen> screens = Screen.AllScreens.ToList();
            Int32 left = screens.Min(x => x.Bounds.Left);
            Int32 top = screens.Min(x => x.Bounds.Top);
            Int32 right = screens.Max(x => x.Bounds.Right);
            Int32 bottom = screens.Max(x => x.Bounds.Bottom);

            Rectangle workingArea = new Rectangle(left, top, right - left, bottom - top);

            return workingArea;
        }

        /// <summary>
        /// Gets the cursor pos.
        /// </summary>
        /// <param name="lpPoint">
        /// The lp point.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetCursorPos(out PointApi lpPoint);

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Windows.Point"/> instance.
        /// </returns>
        private static Point GetCursorPosition()
        {
            PointApi positionRelativeToScreen;

            GetCursorPos(out positionRelativeToScreen);

            return new Point(positionRelativeToScreen.X.ToInt32(), positionRelativeToScreen.Y.ToInt32());
        }

        /// <summary>
        /// Gets the color of the screen pixel.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// A <see cref="System.Windows.Media.Color"/> instance.
        /// </returns>
        private static Color GetScreenPixelColor(Point location)
        {
            Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();

                    try
                    {
                        BitBlt(
                            hDC, 0, 0, 1, 1, hSrcDC, Convert.ToInt32(location.X), Convert.ToInt32(location.Y), (Int32)CopyPixelOperation.SourceCopy);
                    }
                    finally
                    {
                        gsrc.ReleaseHdc();
                        gdest.ReleaseHdc();
                    }
                }
            }

            System.Drawing.Color pixelColor = screenPixel.GetPixel(0, 0);

            return new Color
                   {
                       A = Byte.MaxValue, 
                       R = pixelColor.R, 
                       G = pixelColor.G, 
                       B = pixelColor.B
                   };
        }

        /// <summary>
        /// Sets the cursor pos.
        /// </summary>
        /// <param name="X">
        /// The X.
        /// </param>
        /// <param name="Y">
        /// The Y.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern Boolean SetCursorPos(Int32 X, Int32 Y);

        /// <summary>
        /// Sets the window RGN.
        /// </summary>
        /// <param name="hWnd">
        /// The h WND.
        /// </param>
        /// <param name="hRdn">
        /// The h RDN.
        /// </param>
        /// <param name="bRedraw">
        /// If set to <c>true</c> [b redraw].
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern Int32 SetWindowRgn(IntPtr hWnd, IntPtr hRdn, Boolean bRedraw);

        /// <summary>
        /// Handles the Click event of the BlueLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void BlueLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Blue == 0)
            {
                return;
            }

            CurrentColor.Blue--;
        }

        /// <summary>
        /// Handles the Click event of the BlueMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void BlueMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Blue == Byte.MaxValue)
            {
                return;
            }

            CurrentColor.Blue++;
        }

        /// <summary>
        /// Buttons the capture mouse.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="routedEventArgs">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ButtonCaptureMouse(Object sender, RoutedEventArgs routedEventArgs)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            button.CaptureMouse();

            Mouse.OverrideCursor = button.Cursor;

            // Get the current cursor location so that we can return to this position
            _startCapturePosition = GetCursorPosition();

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                if (button == Pick && _lastPickPosition != default(Point))
                {
                    SetCursorPos(Convert.ToInt32(_lastPickPosition.X), Convert.ToInt32(_lastPickPosition.Y));
                }
                else if (button == Zoom && _lastZoomPosition != default(Point))
                {
                    SetCursorPos(Convert.ToInt32(_lastZoomPosition.X), Convert.ToInt32(_lastZoomPosition.Y));
                }
            }

            if (button == Zoom)
            {
                CalculateZoomLocation();
                CaptureScreen();
            }
        }

        /// <summary>
        /// Buttons the mouse move.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="routedEventArgs">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ButtonMouseMove(Object sender, RoutedEventArgs routedEventArgs)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            if (button.IsMouseCaptured == false)
            {
                return;
            }

            if (button == Pick)
            {
                CaptureScreenPixel();
            }
            else if (button == Zoom)
            {
                CalculateZoomLocation();
            }
        }

        /// <summary>
        /// Picks the release mouse.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="routedEventArgs">
        /// The routed Event Args.
        /// </param>
        private void ButtonReleaseMouse(Object sender, RoutedEventArgs routedEventArgs)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            Mouse.OverrideCursor = null;

            if (button.IsMouseCaptured)
            {
                button.ReleaseMouseCapture();
            }

            if (button == Pick)
            {
                _lastPickPosition = GetCursorPosition();
            }
            else
            {
                _lastZoomPosition = GetCursorPosition();
            }

            if (_startCapturePosition != default(Point))
            {
                // Set position back to the start capture position
                SetCursorPos(Convert.ToInt32(_startCapturePosition.X), Convert.ToInt32(_startCapturePosition.Y));
            }
        }

        /// <summary>
        /// Calculates the zoom area.
        /// </summary>
        private void CalculateZoomLocation()
        {
            Point capturePosition = GetCursorPosition();

            CalculateZoomLocation(capturePosition);
        }

        /// <summary>
        /// Calculates the zoom location.
        /// </summary>
        /// <param name="capturePosition">
        /// The capture position.
        /// </param>
        private void CalculateZoomLocation(Point capturePosition)
        {
            TranslateTransform translateTransform = ((TransformGroup)ZoomImage.RenderTransform).Children.OfType<TranslateTransform>().First();

            // We need to offset the transform by half the dimensions of the zoom viewing area
            Double zoomHeightOffset = ZoomCapture.Height / 2;
            Double zoomWidthOffset = ZoomCapture.Width / 2;

            translateTransform.X = (capturePosition.X * -1) + zoomWidthOffset;
            translateTransform.Y = (capturePosition.Y * -1) + zoomHeightOffset;

            ScaleTransform scaleTransform = ((TransformGroup)ZoomImage.RenderTransform).Children.OfType<ScaleTransform>().First();

            scaleTransform.ScaleX = Properties.Settings.Default.Scale;
            scaleTransform.ScaleY = Properties.Settings.Default.Scale;

            if (ZoomImage.Source != null)
            {
                scaleTransform.CenterX = (ZoomImage.Source.Width / -2) + capturePosition.X;
                scaleTransform.CenterY = (ZoomImage.Source.Height / -2) + capturePosition.Y;
            }
        }

        /// <summary>
        /// Captures the screen.
        /// </summary>
        private void CaptureScreen()
        {
            Rectangle screenArea = CalculateScreenArea();
            Bitmap screenImage = new Bitmap(screenArea.Width, screenArea.Height, PixelFormat.Format32bppArgb);

            using (Graphics destination = Graphics.FromImage(screenImage))
            {
                using (Graphics source = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr sourceDeviceContext = source.GetHdc();
                    IntPtr destinationDeviceContext = destination.GetHdc();

                    try
                    {
                        BitBlt(
                            destinationDeviceContext, 
                            0, 
                            0, 
                            screenImage.Width, 
                            screenImage.Height, 
                            sourceDeviceContext, 
                            0, 
                            0, 
                            (Int32)CopyPixelOperation.SourceCopy);
                    }
                    finally
                    {
                        source.ReleaseHdc();
                        destination.ReleaseHdc();
                    }
                }
            }

            ScreenCapture = screenImage.ToImageSource();
        }

        /// <summary>
        /// Captures the screen pixel.
        /// </summary>
        private void CaptureScreenPixel()
        {
            Point currentPosition = GetCursorPosition();
            Color capturedColor = GetScreenPixelColor(currentPosition);

            CurrentColor.CurrentColor = capturedColor;
        }

        /// <summary>
        /// Handles the Click event of the Close control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void Close_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Creates the image transform.
        /// </summary>
        private void CreateImageTransform()
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            TranslateTransform translateTransform = new TranslateTransform();
            TransformGroup group = new TransformGroup();

            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);

            ZoomImage.RenderTransform = group;
        }

        /// <summary>
        /// Handles the Click event of the GreenLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void GreenLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Green == 0)
            {
                return;
            }

            CurrentColor.Green--;
        }

        /// <summary>
        /// Handles the Click event of the GreenMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void GreenMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Green == Byte.MaxValue)
            {
                return;
            }

            CurrentColor.Green++;
        }

        /// <summary>
        /// Hooks the events.
        /// </summary>
        private void HookEvents()
        {
            Pick.AddHandler(MouseLeftButtonDownEvent, new RoutedEventHandler(ButtonCaptureMouse), true);
            Pick.AddHandler(MouseRightButtonDownEvent, new RoutedEventHandler(ButtonCaptureMouse), true);
            Pick.AddHandler(MouseMoveEvent, new RoutedEventHandler(ButtonMouseMove), true);
            Pick.AddHandler(MouseLeftButtonUpEvent, new RoutedEventHandler(ButtonReleaseMouse), true);
            Pick.AddHandler(MouseRightButtonUpEvent, new RoutedEventHandler(ButtonReleaseMouse), true);
            Zoom.AddHandler(MouseLeftButtonDownEvent, new RoutedEventHandler(ButtonCaptureMouse), true);
            Zoom.AddHandler(MouseRightButtonDownEvent, new RoutedEventHandler(ButtonCaptureMouse), true);
            Zoom.AddHandler(MouseMoveEvent, new RoutedEventHandler(ButtonMouseMove), true);
            Zoom.AddHandler(MouseLeftButtonUpEvent, new RoutedEventHandler(ButtonReleaseMouse), true);
            Zoom.AddHandler(MouseRightButtonUpEvent, new RoutedEventHandler(ButtonReleaseMouse), true);
        }

        /// <summary>
        /// Handles the Click event of the HueLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void HueLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Hue == 0)
            {
                return;
            }

            CurrentColor.Hue--;
        }

        /// <summary>
        /// Handles the Click event of the HueMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void HueMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Hue == CurrentColor.Converter.HslScaleDescription.HueMax)
            {
                return;
            }

            CurrentColor.Hue++;
        }

        /// <summary>
        /// Loads the default skin.
        /// </summary>
        private void LoadDefaultSkin()
        {
            Byte[] skinData = Properties.Resources.DefaultSkin;

            using (Stream skinStream = new MemoryStream(skinData))
            {
                Skin = SkinParser.Load(skinStream, false);
            }
        }

        /// <summary>
        /// The load skin.
        /// </summary>
        /// <param name="skinPath">
        /// The skin path.
        /// </param>
        private void LoadSkin(String skinPath)
        {
            if (String.IsNullOrWhiteSpace(skinPath))
            {
                LoadDefaultSkin();
            }
            else if (File.Exists(skinPath) == false)
            {
                LoadDefaultSkin();
            }
            else
            {
                try
                {
                    Skin = SkinParser.Load(skinPath, false);
                }
                catch (SkinLoadException)
                {
                    if (Skin == null)
                    {
                        // Use the default skin
                        LoadDefaultSkin();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the LuminanceLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void LuminanceLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Luminance == 0)
            {
                return;
            }

            CurrentColor.Luminance--;
        }

        /// <summary>
        /// Handles the Click event of the LuminanceMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void LuminanceMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Luminance == CurrentColor.Converter.HslScaleDescription.LuminanceMax)
            {
                return;
            }

            CurrentColor.Luminance++;
        }

        /// <summary>
        /// Handles the Click event of the Options control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void Options_Click(Object sender, RoutedEventArgs e)
        {
            OptionsWindow options = new OptionsWindow
                                    {
                                        Owner = this, 
                                        OriginalSettings = Properties.Settings.Default
                                    };

            if (options.ShowDialog() == true)
            {
                if (options.OriginalSettings.Scale != options.UpdatedSettings.Scale)
                {
                    CalculateZoomLocation(_lastZoomPosition);
                }

                if (options.OriginalSettings.CurrentSkinPath != options.UpdatedSettings.CurrentSkinPath)
                {
                    LoadSkin(options.UpdatedSettings.CurrentSkinPath);
                }

                options.UpdatedSettings.CopySettings(Properties.Settings.Default);
            }
        }

        /// <summary>
        /// Handles the Click event of the RedLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void RedLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Red == 0)
            {
                return;
            }

            CurrentColor.Red--;
        }

        /// <summary>
        /// Handles the Click event of the RedMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void RedMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Red == Byte.MaxValue)
            {
                return;
            }

            CurrentColor.Red++;
        }

        /// <summary>
        /// Handles the Click event of the SaturationLess control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void SaturationLess_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Saturation == 0)
            {
                return;
            }

            CurrentColor.Saturation--;
        }

        /// <summary>
        /// Handles the Click event of the SaturationMore control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void SaturationMore_Click(Object sender, RoutedEventArgs e)
        {
            if (CurrentColor.Saturation == CurrentColor.Converter.HslScaleDescription.SaturationMax)
            {
                return;
            }

            CurrentColor.Saturation++;
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.
        /// </param>
        private void Window_Closing(Object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.CurrentColor = CurrentColor.Hex;
        }

        /// <summary>
        /// The window_ mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Window_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            // DragMove handles all the window placement automatically!
            DragMove();
        }

        /// <summary>
        /// Handles the MouseWheel event of the image control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.
        /// </param>
        private void image_MouseWheel(Object sender, MouseWheelEventArgs e)
        {
            ScaleTransform scaleTransform = (ScaleTransform)ZoomImage.RenderTransform;
            Double zoom = e.Delta > 0 ? .2 : -.2;
            scaleTransform.ScaleX += zoom;
            scaleTransform.ScaleY += zoom;
        }

        /// <summary>
        /// Gets or sets the color of the current.
        /// </summary>
        /// <value>
        /// The color of the current.
        /// </value>
        public ColorData CurrentColor
        {
            get
            {
                return (ColorData)GetValue(CurrentColorProperty);
            }

            set
            {
                SetValue(CurrentColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ScreenCapture.
        /// </summary>
        /// <value>
        /// The ScreenCapture.
        /// </value>
        public ImageSource ScreenCapture
        {
            get
            {
                return (ImageSource)GetValue(ScreenCaptureProperty);
            }

            set
            {
                SetValue(ScreenCaptureProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Skin.
        /// </summary>
        /// <value>
        /// The Skin.
        /// </value>
        public SkinDefinition Skin
        {
            get
            {
                return (SkinDefinition)GetValue(SkinProperty);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                SkinDefinition previousSkin = (SkinDefinition)GetValue(SkinProperty);
                WindowInteropHelper interopHelper = new WindowInteropHelper(this);

                interopHelper.EnsureHandle();

                IntPtr windowHandle = interopHelper.Handle;

                Debug.WriteLine("window handle: " + windowHandle);

                if (previousSkin != null && previousSkin.Body.MouseOff.Region != null)
                {
                    // Release any previous region applied to the window
                    SetWindowRgn(windowHandle, IntPtr.Zero, false);
                }

                Debug.WriteLine("Window dimensions before " + Height + " - " + Width);
                SetValue(SkinProperty, value);
                Height = value.Body.MouseOff.Height;
                Width = value.Body.MouseOff.Width;
                Debug.WriteLine("Window dimensions after " + Height + " - " + Width);

                if (value.Body.MouseOff.Region != null)
                {
                    Graphics graphics = Graphics.FromHwnd(windowHandle);

                    SetWindowRgn(windowHandle, value.Body.MouseOff.Region.GetHrgn(graphics), true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the ZoomArea.
        /// </summary>
        /// <value>
        /// The ZoomArea.
        /// </value>
        public Rect ZoomArea
        {
            get
            {
                return (Rect)GetValue(ZoomAreaProperty);
            }

            set
            {
                SetValue(ZoomAreaProperty, value);
            }
        }

        /// <summary>
        /// The pointapi.
        /// </summary>
        private struct PointApi
        {
            /// <summary>
            /// The x.
            /// </summary>
            public IntPtr X;

            /// <summary>
            /// The y.
            /// </summary>
            public IntPtr Y;
        }
    }
}