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
    public class BugLogController : ControllerBase
    {
        public ApplicationDbContext _context;
        public BugLogSearchService _bugLogService;

        public BugLogController(ApplicationDbContext context,BugLogSearchService bugLogSearchService)
        {
            _context = context;
            _bugLogService = bugLogSearchService;
        }

        /// <summary>
        /// Returns all bugs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BugLog>>> GetBugLogs()
        {
            return await _context.BugLogs.ToListAsync();
        }

        /// <summary>
        /// Creates a bug
        /// </summary>
        /// <param name="bugLog"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BugLog>> CreateBug(BugLog bugLog)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            _context.BugLogs.Add(bugLog);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBugLogs), new { id = bugLog.Id }, bugLog);
        }

        /// <summary>
        /// Searches for a bug from the db with status or projectId the bug is attached to
        /// </summary>
        /// <param name="status"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IEnumerable<BugLog>> SearchBugLog(string? status, int projectId)
        {
            var result = await _bugLogService.SearchBugLogs(status, projectId);
            return result;
        }

        /// <summary>
        /// Updates the bug status Open/Fixed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateBugLogStatus(int id)
        { 
            var bugs = await _context.BugLogs.FindAsync(id);

            if (bugs == null)
            { 
                return NotFound();
            }

            if (bugs.Status == "Open")
            {
                bugs.Status = "Fixed";
            }
            else 
            {
                bugs.Status = "Open";
            }

            await _context.SaveChangesAsync();
            return Ok(bugs);
        }

        /// <summary>
        /// Change/Update the BugLogs AssignedToId value using Id to find the bug
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assignedToId"></param>
        /// <returns></returns>
        [HttpPatch("{id}/{assignedToId}/updateAssignedUserId")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBugAssignedToId(int id,string assignedToId)
        {
            var bug = await _context.BugLogs.FindAsync(id);

            if (bug == null)
            {
                return NotFound();
            }

            if (bug.AssignedToId != null | bug.AssignedToId == null)
            {
                bug.AssignedToId = assignedToId;
            }
            else
            { 
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return Ok(bug);
        }

        /// <summary>
        /// Deletes a bug log by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteBugLog(int id)
        {
            var bugLog = await _context.BugLogs.FindAsync(id);

            if (bugLog == null)
            {
                return NotFound();
            }

            _context.Remove(bugLog);
            await _context.SaveChangesAsync();

            return Ok(bugLog);
        }
    }
}
