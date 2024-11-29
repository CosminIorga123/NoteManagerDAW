using System;
using System.Runtime.Serialization;

namespace NoteManager.Exceptions
{
    /// <summary>
    /// Exception that is thrown when an input category ID is not within the accepted values.
    /// </summary>
    public class InvalidCategoryId : Exception  
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryId"/> class.
        /// </summary>
        public InvalidCategoryId() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryId"/> class with a specified error message.
        /// </summary>
        /// <param name="id">The invalid category ID.</param>
        public InvalidCategoryId(string id) : base($"Id {id} is undefined. Accepted values are 1,2,3.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryId"/> class with serialized data.
        /// </summary>
        /// <param name="info">The serialized object data.</param>
        /// <param name="ctxt">The contextual information about the source or destination.</param>
        protected InvalidCategoryId(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }

    }
}
