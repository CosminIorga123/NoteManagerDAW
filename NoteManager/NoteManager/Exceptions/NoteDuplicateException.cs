using System;

namespace NoteManager.Exceptions
{
    /// <summary>
    /// Exception that is thrown when attempting to insert a note with an already existing ID.
    /// </summary>
    public class NoteDuplicateException : Exception 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDuplicateException"/> class.
        /// </summary>
        public NoteDuplicateException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDuplicateException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NoteDuplicateException(string message) : base(message) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDuplicateException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public NoteDuplicateException(string message, Exception inner) : base(message, inner) { }
        
    }
}
