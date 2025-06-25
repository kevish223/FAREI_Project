using FAREI_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FormRequest.Models;

namespace FAREI_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<FAREI_Project.Models.Request> Request { get; set; } = default!;
        public DbSet<FormRequest.Models.FormReqDb> FormReqDb { get; set; } = default!;
        public DbSet<FormReqDb> FormReqDbs { get; set; } = default!;
        public DbSet<Registry> Registries { get; set; } = default!;
    }
}
