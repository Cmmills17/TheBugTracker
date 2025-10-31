using TheBugTracker.Client;
using TheBugTracker.Client.Models;
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

        /// <summary>
        /// Retrieve detailed info about a specific project,
        /// if it exists in the user's company
        /// </summary>
        /// <param name="id">The ID of the project to retrieve</param>
        /// <param name="user">The current user's claim</param>
        /// <returns>The requested project, or null if it does not exist</returns>
        public Task<Project?> GetProjectByIdAsync(int id, UserInfo user);

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
        /// Updates the details of a project in the databse, as long as it exists for the users company and, 
        /// they are authorized to do so
        /// </summary>
        /// <remarks>
        /// Users must be in the role of Admin or Project Manage
        /// If the user is a project manager they must be assigned to the
        /// project they are updating
        /// </remarks>
        /// <param name="project">The updated project information</param>
        /// <param name="user">The current user's claims</param>
        public Task UpdateProjectAsync(Project project, UserInfo user);

        /// <summary>
        /// Archives a prokect to market as inactive. This method will aslo
        /// archive all of the tickets associated with the project
        /// </summary>
        /// <remarks>
        /// Projects may only be archived by admins of the project's company 
        /// or the project manager assigned to the project
        /// </remarks>
        /// <param name="projectId">The ID of the project to archive</param>
        /// <param name="user">The claims of the current user</param>
        public Task ArchiveProjectAsync(int projectId, UserInfo user);

        /// <summary>
        /// Restore a prokect to market as active. This method will aslo
        /// restore all of the tickets associated with the project which
        /// were not previously archived.
        /// </summary>
        /// <remarks>
        /// Projects may only be restored by admins of the project's company 
        /// or the project manager assigned to the project
        /// </remarks>
        /// <param name="projectId">The ID of the project to restore</param>
        /// <param name="user">The claims of the current user</param>
        public Task RestoreProjectAsync(int projectId, UserInfo user);

        /// <summary>
        /// Assings a user to the specified project,
        /// if they are not already assigned.
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="userId">The ID of the user</param>
        /// <param name="user">The current users claim</param>
        /// <returns></returns>
        public Task AddProjectMemberAsync(int projectId, string userId, UserInfo user);


        /// <summary>
        /// Remove a user from the specified project,
        /// if they are currently assigned.
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="userId">The ID of the user</param>
        /// <param name="user">The current users claim</param>
        /// <returns></returns>
        public Task RemoveProjectMemberAsync(int projectId, string userId, UserInfo user);

        /// <summary>
        /// Retrieves a list of users currently assigned
        /// to a specific project
        /// </summary>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="user">Claims of the current user</param>
        /// <returns>A collection of users</returns>
        public Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, UserInfo user);


    }
}
