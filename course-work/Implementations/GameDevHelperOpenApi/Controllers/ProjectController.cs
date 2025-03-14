using GameDevHelper.Models;
using GameDevHelperOpenApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameDevHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectSearchService _projectSearchService;

        public ProjectController(ApplicationDbContext context, ProjectSearchService projectSearchService)
        {
            _context = context;
            _projectSearchService = projectSearchService;
        }


        /// <summary>
        /// Returns all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.Include(p => p.Features).Include(p => p.BugLogs).ToListAsync();
        }


        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            if (_context.Projects.Any(p => p.Name == project.Name))
            {
                return BadRequest("Project with the same name already exist!");
            }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }


        /// <summary>
        /// Searches for a Project with id or projectName or status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IEnumerable<Project>> SearchProject(int? id, string? projectName, string? status)
        {
            var result = await _projectSearchService.SearchProject(id, projectName, status);
            return result;
        }


        /// <summary>
        /// Updates the project status In Progress/Finished
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateProjectStatus(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            if (project.Status == "In Progress")
            {
                project.Status = "Fixed";
            }
            else
            {
                project.Status = "In Progress";
            }
            await _context.SaveChangesAsync();
            return Ok(project);
        }


        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteProject(int id)
        { 
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return Ok(project);
        }

    }
}
