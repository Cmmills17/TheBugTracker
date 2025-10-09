using Microsoft.CodeAnalysis;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;
using TheBugTracker.Services.Interfaces;
using TheBugTracker.Models;

using Project = TheBugTracker.Models.Project;
using TheBugTracker.Client;
namespace TheBugTracker.Services
{
    public class ProjectDTOService(IProjectRepository repository) : IProjectDTOService
    {
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user)
        {
            IEnumerable<Project> projects = await repository.GetProjectsAsync(user);

            IEnumerable<ProjectDTO> dtos = projects.Select(p => p.ToDTO());

            return dtos;
        }

      
    }
}
