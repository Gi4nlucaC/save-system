using System.Threading.Tasks;

namespace SaveSystem
{
    /// <summary>
    /// Interface for storage providers (local file system, cloud, etc.)
    /// </summary>
    /// <typeparam name="T">Type of content to store</typeparam>
    public interface ISaveStorage<T>
    {
        /// <summary>
        /// Write content to storage synchronously
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        /// <param name="content">Content to write</param>
        void Write(string slotId, T content);
        
        /// <summary>
        /// Write content to storage asynchronously
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        /// <param name="content">Content to write</param>
        /// <returns>Task representing the async operation</returns>
        Task WriteAsync(string slotId, T content);
        
        /// <summary>
        /// Read content from storage synchronously
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        /// <returns>Content from storage</returns>
        T Read(string slotId);
        
        /// <summary>
        /// Read content from storage asynchronously
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        /// <returns>Task containing content from storage</returns>
        Task<T> ReadAsync(string slotId);
        
        /// <summary>
        /// Check if content exists in storage
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        /// <returns>True if content exists, false otherwise</returns>
        bool Exists(string slotId);
        
        /// <summary>
        /// Delete content from storage
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
        void Delete(string slotId);
    }
}