using System;
using System.Text.Json.Serialization;

namespace NoteManager.EntityModels
{
    /// <summary>
    /// Represents an entity model for a note.
    /// </summary>
    public class NoteEM
    {   
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the owner of the note.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the description of the note.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier for the category to which the note belongs.
        /// </summary>
        public string CategoryId { get; set; }

    }
}
