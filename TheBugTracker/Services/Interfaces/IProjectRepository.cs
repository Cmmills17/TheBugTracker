using TheBugTracker.Client;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Retrieves all projects in the database
        /// </summary>
        /// <param name="user"> The current user's claims</param>
        public Task<IEnumerable<Project>> GetProjectsAsync(UserInfo user);

    }
}
