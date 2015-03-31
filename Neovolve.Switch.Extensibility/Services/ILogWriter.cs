namespace Neovolve.Switch.Extensibility.Services
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="ILogWriter"/>
    ///   interface is used to define the methods for writing log information.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Writes the specified type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        void Write(TraceEventType type, String message, params Object[] arguments);
    }
}