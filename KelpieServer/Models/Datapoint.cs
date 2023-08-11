using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Migrations etc:
// https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

namespace KelpieServer.Models
{ 
    // Full Datapoint -- uses classes from DatapointComponents
    public class Datapoint
    {
        [Key, Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        //public string ProjectId { get; set; } = string.Empty;
        //public Project Project { get; set; } = new Project();

        [Required]
        public long Date { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Authors { get; set; }

        public string? Landform { get; set; }

        public string? Relief { get; set; }

        public string? Slope { get; set; }

        public string? SoilUnit { get; set; }

        public bool? NormalCircumstances { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string? NWI { get; set; }

        [Required, Column(TypeName = "jsonb")]
        public Hydrology Hydrology { get; set; } = new Hydrology();

        [Required, Column(TypeName = "jsonb")]
        public Soil Soil { get; set; } = new Soil();

        [Required, Column(TypeName = "jsonb")]
        public Vegetation Vegetation { get; set; } = new Vegetation();
    }
}