using System;
using System.Runtime.Serialization;

namespace NoteManager.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a note is missing required attributes.
    /// </summary>
    public class NoteIncompleteException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteIncompleteException"/> class.
        /// </summary>
        public NoteIncompleteException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteIncompleteException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NoteIncompleteException(string message): base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteIncompleteException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected NoteIncompleteException(SerializationInfo info, StreamingContext ctxt) { }
    }
}
