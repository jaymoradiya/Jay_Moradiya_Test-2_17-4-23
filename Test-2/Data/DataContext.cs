using Microsoft.EntityFrameworkCore;

namespace Test_2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}