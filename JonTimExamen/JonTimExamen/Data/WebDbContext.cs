using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JonTimExamen.Models;

namespace JonTimExamen.Data
{
    public class WebDbContext : IdentityDbContext<Employee>
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Appointment> Appointment { get; set; }

        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
