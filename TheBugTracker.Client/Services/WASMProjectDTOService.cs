using System.Net.Http.Json;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;

namespace TheBugTracker.Client.Services
{
    public class WASMProjectDTOService(HttpClient http) : IProjectDTOService
    {
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user)
        {
            try
            {
                List<ProjectDTO> projects = await http.GetFromJsonAsync<List<ProjectDTO>>("api/Projects") ?? [];

                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return [];
            }
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id, UserInfo user)
        {
            try
            {
                var project = await http.GetFromJsonAsync<ProjectDTO>($"api/Projects/{id}");

                return project;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null; 
            }
        }

        public Task<ProjectDTO> CreateProjectAsync(ProjectDTO project, UserInfo user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(ProjectDTO project, UserInfo user)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(int projectId, UserInfo user)
        {
            throw new NotImplementedException();
        }

        public Task RestoreProjectAsync(int projectId, UserInfo user)
        {
            throw new NotImplementedException();
        }

    }
}
