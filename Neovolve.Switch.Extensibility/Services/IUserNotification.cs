namespace Neovolve.Switch.Extensibility.Services
{
    using System;

    /// <summary>
    /// The <see cref="IUserNotification"/>
    ///   interface is used to define the methods for prompting the user.
    /// </summary>
    public interface IUserNotification
    {
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void ShowMessage(String message);

        /// <summary>
        /// Asks the question.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        Boolean AskQuestion(String question);
    }
}