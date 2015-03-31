namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Drawing;
    using System.Windows.Media;

    /// <summary>
    /// The <see cref="SkinImage"/>
    ///   class is used to define an image for the skin.
    /// </summary>
    public class SkinImage : SkinPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinImage"/> class.
        /// </summary>
        /// <param name="image">
        /// The image.
        /// </param>
        public SkinImage(SkinImage image)
            : this((SkinPart)image)
        {
            Name = image.Name;
            Source = image.Source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinImage"/> class.
        /// </summary>
        /// <param name="part">
        /// The part.
        /// </param>
        public SkinImage(SkinPart part)
        {
            Top = part.Top;
            Left = part.Left;
            Height = part.Height;
            Width = part.Width;
            Cursor = part.Cursor;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public Region Region
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public ImageSource Source
        {
            get;
            set;
        }
    }
}