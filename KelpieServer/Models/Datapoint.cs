using System.ComponentModel.DataAnnotations;

namespace KelpieServer.Models
{
    public class Hydrology
    {
        [Required]
        public bool Present { get; set; } = false;

        [Required]
        public bool Disturbed { get; set; } = false;

        [Required]
        public bool Problematic { get; set; } = false;

        // surface water
        public float? SurfaceWaterDepth { get; set; }
        public bool? SurfaceWaterPresent { get; set; }

        // water table
        public float? WaterTableDepth { get; set; }
        public bool? WaterTablePresent { get; set; }

        // saturation
        public float? SaturationDepth { get; set; }
        public bool? SaturationPresent { get; set; }

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