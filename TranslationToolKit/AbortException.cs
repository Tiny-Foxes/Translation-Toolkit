using System;
using System.Runtime.Serialization;

namespace TranslationToolKit
{
    /// <summary>
    /// Abort exception, throw this exception to go back to the main menu.
    /// </summary>
    [Serializable]
    internal class AbortException : Exception
    {
        public AbortException()
        {
        }

        public AbortException(string message) : base(message)
        {
        }

        public AbortException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbortException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}