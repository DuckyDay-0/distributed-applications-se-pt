using GameDevHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GameDevHelperOpenApi.Services
{
    public class UserSearchService
    {
        private readonly ApplicationDbContext _context;

        public UserSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="eMail"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User>> SearchUser(string? userName, string? eMail, string? id)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.UserName == userName);
            }

            if (!string.IsNullOrEmpty(eMail))
            {
                query = query.Where(u => u.Email == eMail);
            }

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(u => u.Id == id);
            }

            return await query.ToListAsync();
        }
    }
}
