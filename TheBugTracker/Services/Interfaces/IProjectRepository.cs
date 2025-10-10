using TheBugTracker.Client;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Creates a new project in the database for the users company
        /// </summary>
        /// <remarks>
        /// Only Project Managers and Admins may submit new projects
        /// </remarks>
        /// <param name="project">The project to be saved in the database</param>
        /// <param name="user">The current user's claims</param>
        /// <returns>The created project after it's been saved in the database</returns>
        public Task<Project> CreateProjectAsync(Project project, UserInfo user);

        /// <summary>
        /// Retrieves all projects in the database
        /// </summary>
        /// <param name="user"> The current user's claims</param>
        public Task<IEnumerable<Project>> GetProjectsAsync(UserInfo user);

        /// <summary>
        /// Retrieve detailed info about a specific project,
        /// if it exists in the user's company
        /// </summary>
        /// <param name="id">The ID of the project to retrieve</param>
        /// <param name="user">The current user's claim</param>
        /// <returns>The requested project, or null if it does not exist</returns>
        public Task<Project?> GetProjectByIdAsync(int id, UserInfo user);

        



    }
}
