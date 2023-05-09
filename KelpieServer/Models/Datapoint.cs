using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Migrations etc:
// https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

namespace KelpieServer.Models
{
    public class WaterPresence
    {
        public float? Depth { get; set; }
        public bool Present { get; set; } = false;
    }
    public class Hydrology
    {
        public bool Present { get; set; } = false;
        public bool Disturbed { get; set; } = false;
        public bool Problematic { get; set; } = false;
        public WaterPresence SurfaceWater { get; set; } = new WaterPresence();
        public WaterPresence WaterTable { get; set; } = new WaterPresence();
        public WaterPresence Saturation { get; set; } = new WaterPresence();
        public string[] SecondaryIndicators { get; set; } = Array.Empty<string>();
        public string[] PrimaryIndicators { get; set; } = Array.Empty<string>();

        public string? Remarks { get; set; } = string.Empty;
    }

    public class Datapoint
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int Date { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Authors { get; set; }

        public string? Landform { get; set; }

        public string? Relief { get; set; }

        public string? Slope { get; set; }

        public string? SoilUnit { get; set; }

        public bool? NormalCircumstances { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set;}

        public string? NWI { get; set; }

        [Required, Column(TypeName = "jsonb")]
        public Hydrology Hydrology { get; set; } = new Hydrology();

        // finish everything else once database is running

    }
}