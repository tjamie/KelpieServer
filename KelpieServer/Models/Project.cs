using System.ComponentModel.DataAnnotations;

namespace KelpieServer.Models
{
    public class Project
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public int Date { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Applicant { get; set; }

        public string? County { get; set; }

        public string? State { get; set; }

        public string? Section { get; set; }

        public string? Region { get; set; }

        public string? Subregion { get; set; }

        public string? Datum { get; set; }
    }
}