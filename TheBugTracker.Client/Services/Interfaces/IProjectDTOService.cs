using TheBugTracker.Client.Models;

namespace TheBugTracker.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        /// <summary>
        /// Creates a new project in the database for the users company
        /// from a ProjectDTO
        /// </summary>
        /// <remarks>
        /// Only Project Managers and Admins may submit new projects
        /// </remarks>
        /// <param name="project">The project to be saved in the database</param>
        /// <param name="user">The current user's claims</param>
        /// <returns>The created project after it's been saved in the database</returns>
        public Task<ProjectDTO> CreateProjectAsync(ProjectDTO project, UserInfo user);

        /// <summary>
        /// Retrieves all active projects for the current user's company
        /// </summary>
        /// <returns>An Enumerable of projects</returns>
        /// /// <param name="user"> The current user's claims</param>
        public Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user);

        /// <summary>
        /// Retrieve detailed info about a specific project,
        /// if it exists in the user's company
        /// </summary>
        /// <param name="id">The ID of the project to retrieve</param>
        /// <param name="user">The current user's claim</param>
        /// <returns>The requested project, or null if it does not exist</returns>
        public Task<ProjectDTO?> GetProjectByIdAsync(int id, UserInfo user);

       


    }
}
