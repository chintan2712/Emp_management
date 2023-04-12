using EmployeeManagementProject.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementProject.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Employee> Employee { get; set; }


    }
}
