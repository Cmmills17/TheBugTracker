using TheBugTracker.Client.Models;

namespace TheBugTracker.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        /// <summary>
        /// Retrieves all active projects for the current user's company
        /// </summary>
        /// <returns>An Enumerable of projects</returns>
        /// /// <param name="user"> The current user's claims</param>
        public Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user);



    }
}
