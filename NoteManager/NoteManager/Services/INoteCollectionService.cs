using NoteManager.DtoModels;
using NoteManager.EntityModels;
using NoteManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteManager.Services
{
    public interface INoteCollectionService : ICollectionService<NoteDto>
    {

        /// <summary>
        /// Retrieves a note by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the note.</returns>
        Task<NoteDto> GetNoteById(Guid id);

        /// <summary>
        /// Retrieves a list of notes associated with a specific owner.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the owner.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes.</returns>
        Task<List<NoteDto>> GetNotesByOwnerId(Guid ownerId);
        
    }
}
