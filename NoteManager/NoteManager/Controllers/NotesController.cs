using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using NoteManager.Services;
using System.Threading.Tasks;
using NoteManager.DtoModels;
using NoteManager.Exceptions;
using System.Diagnostics;

namespace NoteManager.Controllers
{
    /// <summary>
    /// Handles the CRUD operations for notes.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteCollectionService _noteCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesController"/> class.
        /// </summary>
        /// <param name="noteCollectionService">The note collection service.</param>
        public NotesController(INoteCollectionService noteCollectionService) {
            _noteCollectionService = noteCollectionService ?? throw new ArgumentNullException(nameof(noteCollectionService));
        }

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <returns>A list of notes.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNotesAsync()
        {
            
            List<NoteDto> notes = await _noteCollectionService.GetAll();
            return Ok(notes);
        }

        // <summary>
        /// Gets a note by ID.
        /// </summary>
        /// <param name="id">The ID of the note to retrieve.</param>
        /// <returns>The requested note.</returns>
        /// <response code="200">Returns the requested note.</response>
        /// <response code="404">If the note is not found.</response>
        [HttpGet("GetNoteById",Name = "GetNote")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid id)
        {
            try 
            {
                NoteDto note = await _noteCollectionService.GetNoteById(id);
                return Ok(note);
            }
            catch(NoteNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
        }
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            List<CategoryDto> categories = await _noteCollectionService.GetAllCategories();
            return Ok(categories);
        }

        /// <summary>
        /// Deletes a note by ID.
        /// </summary>
        /// <param name="id">The ID of the note to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="200">Returns if the note was deleted successfully.</response>
        /// <response code="404">If the note is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoteAsync(Guid id)
        {
            try
            {
                bool response = await _noteCollectionService.Delete(id);
                return Ok();
            }
            catch(NoteNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }



        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryAsync(string id)
        {
            try
            {
                bool response = await _noteCollectionService.DeleteCategory(id);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        // de completat
        [HttpPost("AddCategory")]
        public async Task<IActionResult> CreateCategoryAsync(CategoryDto category)
        {
            try
            {
                CategoryDto createdCategory = await _noteCollectionService.CreateCategory(category);
                return Ok(createdCategory);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note">The note to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="201">Returns if the note was created successfully.</response>
        /// <response code="400">Returns if the note is incomplete.</response>
        /// <response code="409">Returns if a note with the same ID already exists.</response>
        /// <response code="401">Returns if the category ID is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateNoteAsync(NoteDto  note)
        {
            try { 
                NoteDto createdNote = await _noteCollectionService.Create(note);
                return CreatedAtRoute("GetNote", new { id = createdNote.Id }, createdNote);
            }
            catch (NoteIncompleteException e)
            {
                return BadRequest(e.Message + note);
            }   
            catch(NoteDuplicateException e)
            {
                return Conflict(e.Message);
            }
            catch(InvalidCategoryId e)
            {
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="id">The ID of the note to update.</param>
        /// <param name="note">The updated note data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="200">Returns if the note was updated successfully.</response>
        /// <response code="400">Returns if the note is incomplete or the category ID is invalid.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNoteAsync(Guid id, NoteDto note)
        {
            try 
            {
                bool result = await _noteCollectionService.Update(id, note);
                return Ok(result);
            }
            catch (InvalidCategoryId e)
            {
                return BadRequest(e.Message);
            }
            catch(NoteIncompleteException e)
            {
                return BadRequest(e.Message) ;
            }
        }
    }

}