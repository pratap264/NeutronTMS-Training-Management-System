using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using NeutronTMS.Models;

namespace NeutronTMS.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<NeutronTMS.Models.Trainings>? Trainings { get; set; }
    public DbSet<NeutronTMS.Models.Projects>? Projects { get; set; }
    public DbSet<NeutronTMS.Models.Assessment>? Assessment { get; set; }
    public DbSet<NeutronTMS.Models.Feedback>? Feedback { get; set; }
}
