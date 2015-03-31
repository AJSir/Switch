namespace Neovolve.Switch.Skinning
{
    using System;

    /// <summary>
    /// The <see cref="SkinDescription"/>
    ///   class is used to describe a skin.
    /// </summary>
    public class SkinDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinDescription"/> class.
        /// </summary>
        public SkinDescription()
        {
            Author = new SkinAuthor();
        }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public SkinAuthor Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description
        {
            get;
            set;
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
        /// Gets or sets the screenshot.
        /// </summary>
        /// <value>
        /// The screenshot.
        /// </value>
        public SkinImage Screenshot
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public String Version
        {
            get;
            set;
        }
    }
}