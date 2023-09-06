using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KelpieServer.Models
{
    public class Project
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public long Date { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Applicant { get; set; }

        public string? County { get; set; }

        public string? State { get; set; }

        public string? Section { get; set; }

        public string? Region { get; set; }

        public string? Subregion { get; set; }

        public string? Datum { get; set; }

        // Datapoints belonging to a given project
        public ICollection<Datapoint> Datapoints { get; } = new List<Datapoint>();

        // Many-to-many for assigned users
        public List<UserProject> UserProjects { get; } = new();
        public List<User> Users { get; } = new();

        // initialize navigation property lists
        public Project()
        {
            UserProjects = new List<UserProject>();
            Users = new List<User>();
        }
    }
}