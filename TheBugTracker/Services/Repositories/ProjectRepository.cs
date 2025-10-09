using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services.Repositories
{
    public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetProjectsAsync(string userId)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            ApplicationUser? user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return [];
            }

            IEnumerable<Project> projects = await context.Projects
                .Where(p => p.CompanyId == user.CompanyId && p.Archive == false)
                .ToListAsync();

            return projects;
        }
    }
}
