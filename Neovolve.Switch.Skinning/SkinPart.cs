namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// The <see cref="SkinPart"/>
    ///   class is used to define the base class for skin parts.
    /// </summary>
    public class SkinPart
    {
        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        /// <value>
        /// The cursor.
        /// </value>
        public Cursor Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public Double Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public Double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public Double Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public Double Width
        {
            get;
            set;
        }
    }
}