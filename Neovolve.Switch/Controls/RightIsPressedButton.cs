namespace Neovolve.Switch.Controls
{
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    /// <summary>
    /// The <see cref="RightIsPressedButton"/>
    ///   class is used to provide a custom button control that will support setting the <see cref="ButtonBase.IsPressed"/>
    ///   property for the right mouse button.
    /// </summary>
    /// <remarks>The <see cref="ButtonBase.IsPressed"/> property is only set for the left mouse button and the space bar. 
    /// This makes style transformations against this property inconsistent when the right mouse button is used.</remarks>
    public class RightIsPressedButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightIsPressedButton"/> class.
        /// </summary>
        public RightIsPressedButton()
        {
            AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(RightButtonDown), true);
            AddHandler(MouseRightButtonUpEvent, new MouseButtonEventHandler(RightButtonUp), true);
        }

        /// <summary>
        /// Rights the button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void RightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = true;
        }

        /// <summary>
        /// Rights the button up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void RightButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPressed = false;
        }
    }
}