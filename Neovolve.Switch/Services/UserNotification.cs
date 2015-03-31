namespace Neovolve.Switch.Services
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="UserNotification"/>
    ///   class is used to provide simple message prompting to the user.
    /// </summary>
    [Export(typeof(IUserNotification))]
    public class UserNotification : IUserNotification
    {
        /// <summary>
        /// Asks the question.
        /// </summary>
        /// <param name="question">
        /// The question.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        public Boolean AskQuestion(String question)
        {
            return MessageBox.Show(question, "Switch", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void ShowMessage(String message)
        {
            MessageBox.Show(message, "Switch", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}