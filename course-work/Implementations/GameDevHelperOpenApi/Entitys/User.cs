using Microsoft.AspNetCore.Identity;

namespace GameDevHelper.Models
{
    public class User : IdentityUser
    {
        public List<Feature> Features { get; set; } = new List<Feature>();
        public List<BugLog> BugLogs { get; set; } = new List<BugLog>();
    }
}
