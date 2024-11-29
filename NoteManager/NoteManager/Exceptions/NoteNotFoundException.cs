using System;
using System.Runtime.Serialization;

namespace NoteManager.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a note is not found in the database.
    /// </summary>
    public class NoteNotFoundException : Exception
    {
        /// <summary>
        /// Gets the ID of the note that was not found.
        /// </summary>
        public Guid NoteId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteNotFoundException"/> class.
        /// </summary>
        public NoteNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteNotFoundException"/> class with a specified note ID.
        /// </summary>
        /// <param name="id">The ID of the note that was not found.</param>
        public NoteNotFoundException(Guid id): base($"Note with id {id} was not found") {
            NoteId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected NoteNotFoundException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}
