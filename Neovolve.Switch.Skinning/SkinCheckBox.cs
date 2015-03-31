namespace Neovolve.Switch.Skinning
{
    /// <summary>
    /// The <see cref="SkinCheckBox"/>
    ///   class is used to define a checkbox for a skin.
    /// </summary>
    public class SkinCheckBox
    {
        /// <summary>
        /// Gets or sets the checked.
        /// </summary>
        /// <value>
        /// The checked.
        /// </value>
        public SkinImage Checked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the checked mouse over.
        /// </summary>
        /// <value>
        /// The checked mouse over.
        /// </value>
        public SkinImage CheckedMouseOver
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unchecked.
        /// </summary>
        /// <value>
        /// The unchecked.
        /// </value>
        public SkinImage Unchecked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unchecked mouse over.
        /// </summary>
        /// <value>
        /// The unchecked mouse over.
        /// </value>
        public SkinImage UncheckedMouseOver
        {
            get;
            set;
        }
    }
}