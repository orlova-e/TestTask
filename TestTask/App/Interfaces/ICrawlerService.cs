using System;
using TestTask.Domain;

namespace TestTask.App.Interfaces
{
    /// <summary>
    /// Processing an Crawler object and getting the result
    /// </summary>
    public interface ICrawlerService
    {
        /// <summary>
        /// Starts processing the page
        /// </summary>
        /// <param name="url">Url of the page to be processed</param>
        /// <returns>The Id, which can be used to find out the result</returns>
        Guid StartProcessing(string url);

        /// <summary>
        /// Used to return the page processing state
        /// </summary>
        /// <param name="id">Id of the processed or finished result</param>
        /// <returns>Page processing state</returns>
        ProcessStatus GetStatus(Guid id);

        /// <summary>
        /// Used to return the result of page processing
        /// </summary>
        /// <param name="id">Id of the processed or finished result</param>
        /// <returns>Page processing result</returns>
        int GetResult(Guid id);
    }
}
