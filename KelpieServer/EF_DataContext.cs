﻿using KelpieServer.Models;
using Microsoft.EntityFrameworkCore;

namespace KelpieServer
{
    public class EF_DataContext : DbContext, IDataContext
    {
        public EF_DataContext(DbContextOptions<EF_DataContext> options) : base(options) { }
        public DbSet<Datapoint> Datapoints { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProject> UserProject { get; set; }

        // IDataContext implementation
        public void MarkAsModified(User user)
        {
            Entry(user).State = EntityState.Modified;
        }
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Assign UserProject keys -- many-to-many relationship between Users and Projects
            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects)
                .WithMany(e => e.Users)
                .UsingEntity<UserProject>();

            modelBuilder.UseSerialColumns();
        }
    }
}
