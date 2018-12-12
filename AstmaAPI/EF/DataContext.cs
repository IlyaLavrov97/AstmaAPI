using AstmaAPI.Models;
using AstmaAPI.Models.DBO;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.EF
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ChartValue> ChartValues { get; set; }
    }
}
