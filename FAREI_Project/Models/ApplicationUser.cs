using Microsoft.AspNetCore.Identity;

namespace FAREI_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String? Type { get; set; }
        public String? Site { get; set; }
        public String? Supervisor { get; set; }
    }
}
