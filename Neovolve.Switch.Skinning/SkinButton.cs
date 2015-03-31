namespace Neovolve.Switch.Skinning
{
    /// <summary>
    /// The <see cref="SkinButton"/>
    ///   class is used to define a button for a skin.
    /// </summary>
    public class SkinButton
    {
        /// <summary>
        /// Gets or sets the mouse down.
        /// </summary>
        /// <value>
        /// The mouse down.
        /// </value>
        public SkinImage MouseDown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mouse off.
        /// </summary>
        /// <value>
        /// The mouse off.
        /// </value>
        public SkinImage MouseOff
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mouse over.
        /// </summary>
        /// <value>
        /// The mouse over.
        /// </value>
        public SkinImage MouseOver
        {
            get;
            set;
        }
    }
}