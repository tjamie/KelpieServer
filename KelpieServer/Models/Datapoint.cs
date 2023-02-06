using System.ComponentModel.DataAnnotations;

namespace KelpieServer.Models
{
    public class Hydrology
    {
        public class WaterPresence
        {
            public float? Depth { get; set; }

            [Required]
            public bool Present { get; set; } = false;
        }

        [Required]
        public bool Present { get; set; } = false;

        [Required]
        public bool Disturbed { get; set; } = false;

        [Required]
        public bool Problematic { get; set; } = false;

        [Required]
        public WaterPresence SurfaceWater { get; set; } = new WaterPresence();

        [Required]
        public WaterPresence WaterTable { get; set; } = new WaterPresence();

        [Required]
        public WaterPresence Saturation { get; set; } = new WaterPresence();

        [Required]
        public string[] SecondaryIndicators { get; set; } = Array.Empty<string>();

        [Required]
        public string[] PrimaryIndicators { get; set; } = Array.Empty<string>();

        public string? Remarks { get; set; } = string.Empty;
    }
        
    public class Datapoint
    {
        [Required]
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

        [Required]
        public Hydrology Hydrology { get; set; } = new Hydrology();

        // finish everything else once database is running

    }
}