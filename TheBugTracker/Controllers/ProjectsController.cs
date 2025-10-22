using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBugTracker.Client;
using TheBugTracker.Client.Helpers;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProjectsController(IProjectDTOService projectService) : ControllerBase
    {
        UserInfo UserInfo => UserInfoHelper.GetUserInfo(User)!;


        /// <summary>
        /// Get Projects
        /// </summary>
        /// <remarks>
        /// Returns All active projets belonging to the user's company.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects()
        {
            var projects = await projectService.GetProjectsAsync(UserInfo);
            return Ok(projects);
        }


        /// <summary>
        /// Get Project By Id
        /// </summary>
        /// <param name="projectId">The ID of the project to retrieve</param>
        /// <remarks>
        /// Get detailed information about a *specific* project, 
        /// if it exists and the user is authorized to view it.
        /// </remarks>
        [HttpGet("{projectId:int}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectByID([FromRoute] int projectId)
        {
            ProjectDTO? project = await projectService.GetProjectByIdAsync(projectId, UserInfo);

            if(project is null)
            {
                return NotFound();
            }

            return Ok(project);
        }


        /// <summary>
        /// Create Project
        /// </summary>
        /// <remarks>
        /// Create a new project for the user's company
        /// 
        /// User's must be a project manager or admin to submit a new 
        /// project. If the user is a project manager, they will be 
        /// assigned to the sibmitted project
        /// </remarks>
        /// <param name="project"> The details of the project to be created</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateProject([FromBody] ProjectDTO project)
        {
            ProjectDTO createdProject = await projectService.CreateProjectAsync(project, UserInfo);

            return CreatedAtAction(
                actionName: nameof(GetProjectByID),
                routeValues: new {projectId = createdProject.Id},
                value: createdProject
            );
        }
    }
}
