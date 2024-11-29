using NoteManager.DtoModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteManager.Services
{
    /// <summary>
    /// Defines a generic contract for basic CRUD operations on a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface ICollectionService<T>
    {
        /// <summary>
        /// Retrieves all items in the collection.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all items.</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Retrieves a specific item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the item.</returns>
        Task<T> Get(Guid id);

        /// <summary>
        /// Creates a new item in the collection.
        /// </summary>
        /// <param name="model">The item to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created item.</returns>
        Task<T> Create(T model);

        /// <summary>
        /// Updates an existing item identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to update.</param>
        /// <param name="model">The updated item.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
        Task<bool> Update(Guid id, T model);

        /// <summary>
        /// Deletes an item identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        Task<bool> Delete(Guid id);
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> CreateCategory(CategoryDto categoryDto);
        Task<bool> DeleteCategory(string id);
    }
}
