using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KelpieServer.Models
{
    public partial class Project
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

        //[ForeignKey("ProjectId")]
        //public ICollection<Datapoint> Datapoints { get; set; }
        public ICollection<Datapoint> Datapoints { get; } = new List<Datapoint>();
    }
}