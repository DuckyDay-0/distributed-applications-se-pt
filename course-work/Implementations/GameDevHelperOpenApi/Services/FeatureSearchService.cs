using GameDevHelper.Models;
using GameDevHelperOpenApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameDevHelper.Models;

namespace GameDevHelperOpenApi.Services
{
    public class FeatureSearchService
    {
        private readonly ApplicationDbContext _context;

        public FeatureSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Feature>> SearchFeatures(int? id = null, string? name = null, string? status = null)
        {
            var query = _context.Features.AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(f => f.Id == id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(f => f.Id == id);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(f => f.Status == status);
            }

            return await query.ToListAsync();
        }
    }
}
