using System;
using System.Collections.Generic;

namespace NoteManager.Models
{
    public class Note
    {
      private string _title, _description, _categoryId;

      private Guid _id, _ownerId;

        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get => _title; set => _title = value; }

        /// <summary>
        /// Gets or sets the description of the note.
        /// </summary>
        public string Description { get => _description; set => _description = value; }

        /// <summary>
        /// Gets or sets the identifier for the category to which the note belongs.
        /// </summary>
        public string CategoryId { get => _categoryId; set => _categoryId = value; }

        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
        public Guid Id { get => _id; set => _id = value; }


        /// <summary>
        /// Gets or sets the unique identifier for the owner of the note.
        /// </summary>
        public Guid OwnerId { get => _ownerId; set => _ownerId = value;}
    }
}
