using AstmaAPI.Models;
using AstmaAPI.Models.DBO;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.EF
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ChartValue> ChartValues { get; set; }
        public DbSet<PeakFlowmetryBound> PeakFlowmetryBounds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PeakFlowmetryBound>()
                        .Property(f => f.Id)
                        .ValueGeneratedOnAdd();
        }
    }
}
