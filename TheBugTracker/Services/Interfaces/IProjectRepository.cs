using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Retrieves all projects in the database
        /// </summary>
        public Task<IEnumerable<Project>> GetProjectsAsync(string userId);

    }
}
