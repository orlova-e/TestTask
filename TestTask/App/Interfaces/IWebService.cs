using System.Threading.Tasks;

namespace TestTask.App.Interfaces
{
    /// <summary>
    /// Works with an HTML document
    /// </summary>
    /// <remarks>To get a new page, call the <see cref="TryGetPageAsync(string)"/> method again</remarks>
    public interface IWebService
    {
        /// <summary>
        /// Tries to get html
        /// </summary>
        /// <param name="url">Page url</param>
        /// <returns><see langword="true"/> if the page was received; otherwise, <see langword="false"/></returns>
        Task<bool> TryGetPageAsync(string url);

        /// <summary>
        /// Retrieves the page text without html tags
        /// </summary>
        /// <returns>The page text</returns>
        string GetText();
    }
}
