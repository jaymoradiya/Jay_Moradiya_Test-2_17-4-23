using Microsoft.EntityFrameworkCore;
using Test_2.Entity;

namespace Test_2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}