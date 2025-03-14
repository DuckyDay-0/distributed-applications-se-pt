using GameDevHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        public ApplicationDbContext _context;


        public TokensController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Generates a JWT token for the logged user 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] TokenDTO model)
        {
            
            User u = _context.Users
                .Where(i => i.UserName == model.Username)                      
                .FirstOrDefault();

            if (u == null)
            {
                return BadRequest(ModelState);
            }

            var claims = new Claim[]
            {
                new Claim("LoggedUserId", u.Id.ToString())
            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("NababahvarchilotOInadqdokormiloto"));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "MSI",
                "GameDevHelperApi",
                claims,
                expires: DateTime.Now.AddMinutes(50),
                signingCredentials: signingCredentials
            );
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.WriteToken(token);

            return Ok(
                new
                {
                    success = true,
                    token = jwtToken
                });
        }
    }
}