using GameDevHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameDevHelperOpenApi.Services
{
    public class ProjectSearchService
    {
        private readonly ApplicationDbContext _context;

        public ProjectSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Project>> SearchProject(int? id = null, string? projectName = null, string? status = null)
        {
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                query = query.Where(p => p.Name == projectName);
            }

            if (id.HasValue)
            {
                query = query.Where(p => p.Id == id);
            }          

            return await query.ToListAsync();
        }
    }
}
