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

            if(isPm == true)
            {
                ApplicationUser projectManager = await context.Users.FirstAsync(u => u.Id == user.UserId);
                project.Members.Add(projectManager);
            }

            context.Add(project);
            await context.SaveChangesAsync();

            return project;

        }
    }
}
