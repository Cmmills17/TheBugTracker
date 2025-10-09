using TheBugTracker.Client.Models;

namespace TheBugTracker.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        /// <summary>
        /// Retrieves all projects in the database
        /// </summary>
        /// <returns>An Enumerable of projects</returns>
        public Task<IEnumerable<ProjectDTO>> GetProjectsAsync(string userId);



    }
}
