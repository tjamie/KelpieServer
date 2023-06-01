using KelpieServer.Models;
using Microsoft.EntityFrameworkCore;

namespace KelpieServer
{
    public class EF_DataContext : DbContext
    {
        public EF_DataContext(DbContextOptions<EF_DataContext> options) : base(options) { }
        public DbSet<Datapoint> Datapoints { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Npgsql doesn't support EF Core's JSON
            //modelBuilder.Entity<Datapoint>().OwnsOne(
            //    datapoint => datapoint.Hydrology, ownedNavigationBuilder =>
            //    {
            //        ownedNavigationBuilder.ToJson();
            //        ownedNavigationBuilder.OwnsOne(hydrology => hydrology.SurfaceWater);
            //        ownedNavigationBuilder.OwnsOne(hydrology => hydrology.WaterTable);
            //        ownedNavigationBuilder.OwnsOne(hydrology => hydrology.Saturation);
            //    });
            modelBuilder.UseSerialColumns();
        }
    }
}
