namespace Neovolve.Switch.Skinning
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="SkinLoadException"/>
    ///   is used to identify that a <see cref="SkinDefinition"/> has failed to load.
    /// </summary>
    [Serializable]
    public class SkinLoadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoadException"/> class.
        /// </summary>
        public SkinLoadException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoadException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public SkinLoadException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoadException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public SkinLoadException(String message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoadException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null. 
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). 
        /// </exception>
        protected SkinLoadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}