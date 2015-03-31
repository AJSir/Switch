namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Color = System.Drawing.Color;

    /// <summary>
    /// The <see cref="BitmapExtensions"/>
    ///   class is used to provide extension methods for the <see cref="Bitmap"/> type.
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Gets the region for the image using the specified transparent color.
        /// </summary>
        /// <param name="image">
        /// The image.
        /// </param>
        /// <param name="transparentColor">
        /// Color of the transparent.
        /// </param>
        /// <returns>
        /// A <see cref="Region"/> instance.
        /// </returns>
        public static Region GetRegion(this Bitmap image, Color transparentColor)
        {
            Color matchColor = Color.FromArgb(transparentColor.R, transparentColor.G, transparentColor.B);

            if (matchColor.Equals(Color.Empty))
            {
                return null;
            }

            Region region = new Region();
            Rectangle rc = new Rectangle(0, 0, 0, 0);
            Boolean isInImage = false;

            region.MakeEmpty();

            for (Int32 y = 0; y < image.Height; y++)
            {
                for (Int32 x = 0; x < image.Width; x++)
                {
                    if (isInImage)
                    {
                        if (image.GetPixel(x, y) == matchColor)
                        {
                            isInImage = false;
                            rc.Width = x - rc.X;
                            region.Union(rc);
                        }
                    }
                    else
                    {
                        if (image.GetPixel(x, y) != matchColor)
                        {
                            isInImage = true;
                            rc.X = x;
                            rc.Y = y;
                            rc.Height = 1;
                        }
                    }
                }

                if (isInImage)
                {
                    isInImage = false;
                    rc.Width = image.Width - rc.X;
                    region.Union(rc);
                }
            }

            return region;
        }

        /// <summary>
        /// Toes the image source.
        /// </summary>
        /// <param name="bitmap">
        /// The bitmap.
        /// </param>
        /// <returns>
        /// A <see cref="ImageSource"/> instance.
        /// </returns>
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
            IntPtr intPtrHBitmap = bitmap.GetHbitmap();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(intPtrHBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(intPtrHBitmap);
            }
        }

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <param name="objectHandle">
        /// The object handle.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern Boolean DeleteObject(IntPtr objectHandle);
    }
}