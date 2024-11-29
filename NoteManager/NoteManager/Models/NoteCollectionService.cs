using AutoMapper;
using MongoDB.Driver;
using NoteManager.DtoModels;
using NoteManager.EntityModels;
using NoteManager.Exceptions;
using NoteManager.Services;
using NoteManager.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteManager.Models
{
    /// <summary>
    /// Provides CRUD operations for managing notes in a MongoDB database.
    /// </summary>
    public class NoteCollectionService : INoteCollectionService
    {
        private readonly IMongoCollection<NoteEM> _notes;
        private readonly IMongoCollection<CategoryEM> _categories;
        private readonly IMapper _mapper;
        private List<String> acceptedCategoryIds = new List<string>{ "1", "2", "3" };

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteCollectionService"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public NoteCollectionService(IMongoDBSettings settings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _notes = database.GetCollection<NoteEM>(settings.NoteCollectionName);
            _categories = database.GetCollection<CategoryEM>(settings.CategoryCollectionName);
        }


        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of categories.</returns>
        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var result = await _categories.FindAsync(category => true);
            return result.ToList().Select(_mapper.Map<CategoryDto>).ToList();
        }

        // de completat
        public async Task<CategoryDto> CreateCategory(CategoryDto category)
        {
            //await _notes.InsertOneAsync(_mapper.Map<NoteEM>(note));
            await _categories.InsertOneAsync(_mapper.Map<CategoryEM>(category));
            return category;
        }

        // de completat
        public async Task<bool> DeleteCategory(string id)
        {
            var result = await _categories.DeleteOneAsync(category => category.Id == id);
            if (!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new Exception(id);
            }
            return true;
        }

        /// <summary>
        /// Retrieves all notes from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes.</returns>
        public async Task<List<NoteDto>> GetAll()
        {
            var  result = await _notes.FindAsync(note => true);
            return result.ToList().Select(_mapper.Map<NoteDto>).ToList();
        }

        public async Task<NoteDto> Get(Guid id)
        {
            NoteEM result = (NoteEM)await _notes.FindAsync(note => note.Id == id);
            return _mapper.Map<NoteDto>(result);
        }

        /// <summary>
        /// Creates a new note in the database.
        /// </summary>
        /// <param name="note">The note to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created note.</returns>
        /// <exception cref="NoteIncompleteException">Thrown when the note is incomplete.</exception>
        /// <exception cref="InvalidCategoryId">Thrown when the category ID is invalid.</exception>
        /// <exception cref="NoteDuplicateException">Thrown when a note with the same title or ID already exists.</returns>
        public async Task<NoteDto> Create(NoteDto note)
        {

            if(note.Id == Guid.Empty)
            {
                note.Id = Guid.NewGuid();
            }
            if (string.IsNullOrWhiteSpace(note.Title) || string.IsNullOrWhiteSpace(note.Description)) //check if any field is null or empty
            {
                throw new NoteIncompleteException("Note incomplete");
            }
            if (!acceptedCategoryIds.Contains(note.CategoryId)) //check if category is accepted
            {
                throw new InvalidCategoryId(note.CategoryId);
            }

            var existingNote = await _notes.Find(n => n.Title == note.Title).FirstOrDefaultAsync();
            if (existingNote != null)
            {
                throw new NoteDuplicateException("A note with the same title already exists");
            }
            try
            { 
                await _notes.InsertOneAsync(_mapper.Map<NoteEM>(note));
                return note;
            }
            catch(MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new NoteDuplicateException("A note with the same ID already exists");
            }
        }

        /// <summary>
        /// Updates an existing note identified by its unique identifier with data from the provided note DTO.
        /// </summary>
        /// <param name="id">The unique identifier of the note to update.</param>
        /// <param name="noteDto">The updated note data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
        /// <exception cref="InvalidCategoryId">Thrown when the category ID is invalid.</exception>
        /// <exception cref="NoteIncompleteException">Thrown when the note is incomplete.</exception>
        public async Task<bool> Update(Guid id, NoteDto noteDto)
        {
            noteDto.Id = id;
            NoteEM note = _mapper.Map<NoteEM>(noteDto);
            if (!acceptedCategoryIds.Contains(note.CategoryId)) //check if category is accepted
            {
                throw new InvalidCategoryId(note.CategoryId);
            }
            if (string.IsNullOrEmpty(note.Title) || string.IsNullOrEmpty(note.Description)) //check if any field is null or empty
            {
                throw new NoteIncompleteException("Note incomplete");
            }
            var result = await _notes.ReplaceOneAsync(note => note.Id == id, note);
            if(!result.IsAcknowledged && result.ModifiedCount == 0)
            {
                await _notes.InsertOneAsync(note);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Retrieves a list of notes associated with a specific owner.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the owner.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes.</returns>
        public async Task<List<NoteDto>> GetNotesByOwnerId(Guid ownerId)
        {
            List<NoteEM> notes = (List<NoteEM>)await _notes.FindAsync(note => note.OwnerId == ownerId);
            return (notes.Select(_mapper.Map<NoteDto>).ToList());
        }


        /// <summary>
        /// Deletes a note identified by its unique identifier from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the note to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        /// <exception cref="NoteNotFoundException">Thrown when the note with the specified ID is not found.</exception>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _notes.DeleteOneAsync(note => note.Id == id);
            if(!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new NoteNotFoundException(id);
            }
            return true;
        }


        // <summary>
        /// Retrieves a note by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the note.</returns>
        /// <exception cref="NoteNotFoundException">Thrown when the note with the specified ID is not found.</exception>
        public async Task<NoteDto> GetNoteById(Guid id)
        {
            NoteEM note = await _notes.Find(note =>note.Id == id).FirstOrDefaultAsync();
            if(note == null)
            {
                throw new NoteNotFoundException(id);
            }
            return _mapper.Map<NoteDto>(note);
        }
    }
}
