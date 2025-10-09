using Microsoft.CodeAnalysis;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;
using TheBugTracker.Services.Interfaces;
using TheBugTracker.Models;

using Project = TheBugTracker.Models.Project;
namespace TheBugTracker.Services
{
    public class ProjectDTOService(IProjectRepository repository) : IProjectDTOService
    {
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(string userId)
        {
            IEnumerable<Project> projects = await repository.GetProjectsAsync(userId);

            IEnumerable<ProjectDTO> dtos = projects.Select(p => p.ToDTO());

            return dtos;
        }

      
    }
}
