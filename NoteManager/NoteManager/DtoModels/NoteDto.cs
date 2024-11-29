using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NoteManager.Models;
using System;

namespace NoteManager.DtoModels
{
    public class NoteDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
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
