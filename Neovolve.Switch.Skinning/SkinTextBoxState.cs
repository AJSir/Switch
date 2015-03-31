namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using FontStyle = System.Windows.FontStyle;

    /// <summary>
    /// The <see cref="SkinTextBoxState"/>
    ///   class is used to define the state for a <see cref="SkinTextBox"/>.
    /// </summary>
    public class SkinTextBoxState : SkinPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinTextBoxState"/> class.
        /// </summary>
        /// <param name="part">
        /// The part.
        /// </param>
        public SkinTextBoxState(SkinPart part)
        {
            Top = part.Top;
            Left = part.Left;
            Height = part.Height;
            Width = part.Width;
            Cursor = part.Cursor;
        }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        public Brush Background
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        public FontFamily FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public Double FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        public FontStyle FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>
        /// The font weight.
        /// </value>
        public FontWeight FontWeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public Brush Foreground
        {
            get;
            set;
        }
    }
}