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
    public class FeatureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FeatureSearchService _searchFeaturesService;
        public FeatureController(ApplicationDbContext context,FeatureSearchService searchFeaturesService)
        {
            _context = context;
            _searchFeaturesService = searchFeaturesService;
        }

        /// <summary>
        /// Returns all features
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Feature>>> GetFeatures()
        {
            return await _context.Features.Include(f => f.Project).ToListAsync();
        }


        /// <summary>
        /// Creates a feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Feature>> CreateFeature(Feature feature)
        {
            if (_context.Features.Any(f => f.Name == feature.Name))
            {
                return BadRequest("A faeture with the same name already exist!");
            }
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFeatures), new { id = feature.Id }, feature);
        }


        /// <summary>
        /// Updates the features status InProgress/Finished
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateFeatureStatus(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            if (feature.Status == "In Progress")
            {
                feature.Status = "Finished";
            }
            else
            {
                feature.Status = "In Progress";
            }
            await _context.SaveChangesAsync();

            return Ok(feature);
        }


        /// <summary>
        /// Change/Update the Features AssignedToId value using Id to find the bug
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assignedUserId"></param>
        /// <returns></returns>
        [HttpPatch("{id}/{assignedUserId}/updateAssignedUserId")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFeatureAssignedUserId(int id,string assignedUserId)
        {
            var feature = await _context.Features.FindAsync(id);

            if (feature == null)
            {
                return NotFound();
            }

            if (assignedUserId != null || assignedUserId == null)
            {
                feature.AssignedToId = assignedUserId;
            }
            else
            { 
                return BadRequest();
            }

            await _context.SaveChangesAsync();
            return Ok(feature);
        
        }

        /// <summary>
        /// Searches for a feature from the db with id or name or status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IEnumerable<Feature>> SearchFeature(int? id = null, string? name = null, string? status = null)
        { 
            var result = await _searchFeaturesService.SearchFeatures(id, name, status);
            return result;
        }

        /// <summary>
        /// Delete feature by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var feature = await _context.Features.FindAsync(id);

            if (feature == null)
            {
                return NotFound();
            }

            _context.Remove(feature);
            await _context.SaveChangesAsync();
            return Ok(feature);
        }
    }
}
