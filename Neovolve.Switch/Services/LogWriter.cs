namespace Neovolve.Switch.Services
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Neovolve.Switch.Extensibility.Services;
    using Neovolve.Toolkit;

    /// <summary>
    /// The <see cref="LogWriter"/>
    ///   class is used to write log messages out to the Enterprise Library logger implementation.
    /// </summary>
    [Export(typeof(ILogWriter))]
    internal class LogWriter : ILogWriter
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
        public void Write(TraceEventType type, String message, params Object[] arguments)
        {
            String category = LogCategory.General;

            if (type <= TraceEventType.Error)
            {
                category = LogCategory.Exception;
            }

            if (arguments != null && arguments.Length > 0)
            {
                message = message.FormatNullMasks(CultureInfo.InvariantCulture, arguments);
            }

            Logger.Write(message, category, 0, 0, type);
        }
    }
}