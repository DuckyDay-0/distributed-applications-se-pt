using GameDevHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameDevHelperOpenApi.Services
{
    public class BugLogSearchService
    {
        private readonly ApplicationDbContext _context;

        public BugLogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<BugLog>> SearchBugLogs(string? status = null, int? projectId = null)
        {
           
            var query = _context.BugLogs.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            if (projectId != 0)
            {
                query = query.Where(p => p.ProjectId == projectId.Value);
            }

            List<BugLog> bugLogs = await query.ToListAsync();

            return await query.ToListAsync();
        }
    }
}
