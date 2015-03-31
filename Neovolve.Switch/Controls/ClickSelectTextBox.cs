namespace Neovolve.Switch.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// The <see cref="ClickSelectTextBox"/>
    ///   class is used to provide a <see cref="TextBox"/> control that selects the text on keyboard or mouse focus.
    /// </summary>
    public class ClickSelectTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClickSelectTextBox"/> class.
        /// </summary>
        public ClickSelectTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }

        /// <summary>
        /// Selects all text.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private static void SelectAllText(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = e.OriginalSource as TextBox;

            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        /// <summary>
        /// Selectivelies the ignore mouse button.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private static void SelectivelyIgnoreMouseButton(Object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;

            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                TextBox textBox = (TextBox)parent;

                if (textBox.IsKeyboardFocusWithin == false)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }
    }
}