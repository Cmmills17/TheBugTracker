using Microsoft.EntityFrameworkCore;
using TheBugTracker.Client;
using TheBugTracker.Client.Models.Enums;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services.Repositories
{
    public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetProjectsAsync(UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects
                .Where(p => p.CompanyId == user.CompanyId && p.Archive == false)
                .ToListAsync();

            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(int id, UserInfo user)
        {

            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.SubmitterUser)
                .Include(t => t.Tickets)
                    .ThenInclude(t => t.DeveloperUser)
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id && p.CompanyId == user.CompanyId);

            return project;
        }


        public async Task<Project> CreateProjectAsync(Project project, UserInfo user)
        {

            bool isAdmin = user.Roles.Any(r => r == nameof(Role.Admin));
            bool isPm = user.Roles.Any(r => r == nameof(Role.ProjectManager));

            if (!isAdmin && !isPm)
            {
                throw new ApplicationException(
                    $"User {user.Email} is not authorized to create a project because they are not an Admin or Project Manager"
                    );
            }

            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            project.CompanyId = user.CompanyId;
            project.Created = DateTimeOffset.UtcNow;

            if (isPm == true)
            {
                ApplicationUser projectManager = await context.Users.FirstAsync(u => u.Id == user.UserId);
                project.Members.Add(projectManager);
            }

            context.Add(project);
            await context.SaveChangesAsync();

            return project;

        }

        public async Task UpdateProjectAsync(Project project, UserInfo user)
        {

            bool canEditProject = await CanUserEditProject(project.Id, user); 

            if (canEditProject)
            {
                await using ApplicationDbContext context = contextFactory.CreateDbContext();

                // Clear out navigation properties so we don't
                // update them by mistake
                project.Members = [];
                project.Tickets = [];

                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }

        public async Task ArchiveProjectAsync(int projectId, UserInfo user)
        {
            bool canEdit = await CanUserEditProject(projectId, user);

            if(canEdit == false)
            {
                return;
            }

            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project project = await context.Projects
                .Include(p => p.Tickets)
                .FirstAsync(p => p.Id == projectId && p.CompanyId == user.CompanyId);

            project.Archive = true;

            foreach(Ticket ticket in project.Tickets)
            {

                // if ticket.Archived == true, the ticket was archived by a user
                // if ticket.Archived == false, then the ticket will be archived by the project
                ticket.ArchivedByProject = !ticket.Archived;
                ticket.Archived = true;

            }

            await context.SaveChangesAsync();
        }

        public async Task RestoreProjectAsync(int projectId, UserInfo user)
        {
            bool canEditProject = await CanUserEditProject(projectId, user);

            if (canEditProject == false)
            {
                return;
            }

            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project project = await context.Projects
                       .Include(p => p.Tickets)
                       .FirstAsync(p => p.Id == projectId && p.CompanyId == user.CompanyId);

            project.Archive = false;

            foreach (Ticket ticket in project.Tickets)
            {
                // if archiveByProject == true, the ticket should no longer be archived
                // if ArchivedByProject == false, the ticket was archived before the project and so
                // should remain archived
                ticket.Archived = !ticket.ArchivedByProject;
                ticket.ArchivedByProject = false;
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks that the project exists, that it belongs to the user's company,
        /// and the user is either an admin or the project manager
        /// that is assigned to the project
        /// </summary>
        /// <param name="projectId">Id of the project to check</param>
        /// <param name="userInfo">The claims of the user to authorize</param>
        /// <returns>True if the user is authorized to edit the project, or false if they are not</returns>
        private async Task<bool> CanUserEditProject(int projectId, UserInfo userInfo)
        {
            bool isAdmin = userInfo.Roles.Any(r => r == nameof(Role.Admin));
            bool isPm = userInfo.Roles.Any(r => r == nameof(Role.ProjectManager));

            // is the user an admin or PM?
            if (isAdmin == false && isPm == false)
            {
                return false;
            }

            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool canEditProject = await context.Projects
                // The project must exist and belong to their company
                .Where(p => p.Id == projectId && p.CompanyId == userInfo.CompanyId)
                // Must be an Admin or PM
                .AnyAsync(p => isAdmin || p.Members.Any(m => m.Id == userInfo.UserId));
                
            return canEditProject;

        }


    }
}
