using KelpieServer.Models;
using Microsoft.EntityFrameworkCore;

namespace KelpieServer
{
    public interface IDataContext : IDisposable
    {
        public DbSet<Datapoint> Datapoints { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProject> UserProject { get; set; }

        void MarkAsModified(User user);
        Task<int> SaveChangesAsync();
    }
}
