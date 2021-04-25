using System;

namespace TestTask.Repo.Interfaces
{
    /// <summary>
    /// In-memory storage
    /// </summary>
    /// <typeparam name="T">Type of stored object</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Checks if the item exists in the storage
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns><see langword="true"/> if the item exists; otherwise, <see langword="false"/></returns>
        bool HasEntry(Guid id);

        /// <summary>
        /// Returns a value from the storage
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Returns a value from the storage if the item exists; otherwise, <see langword="null"/></returns>
        T GetEntry(Guid id);

        /// <summary>
        /// Sets or updates an item in the storage
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="value">Item</param>
        /// <returns>Set or updated value</returns>
        T SetOrUpdateEntry(Guid id, T value);
    }
}
