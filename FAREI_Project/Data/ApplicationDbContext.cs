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
        public DbSet<FormReqDb> FormReqDb { get; set; } = default!;
        public DbSet<Registry> Registries { get; set; } = default!;
        public DbSet<ApplicationUser> Alluser { get; set; }
        public DbSet<EquipmentInventory> Equipment { get; set; } = default!;
        public DbSet<Third_Party> Third_Parties { get; set; } = default!;
        public DbSet<ITTreport> ITTreport { get; set; } = default!;
    }
}
