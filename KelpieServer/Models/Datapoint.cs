using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Migrations etc:
// https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

namespace KelpieServer.Models
{
    // Hydrology JSON
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
        public string Remarks { get; set; } = string.Empty;
    }

    // Vegetation JSON
    public class Plant
    {
        public string Id { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Indicator { get; set; } = string.Empty;
        public int Cover { get; set; } = 0;
        public bool Dominant { get; set; } = false;
    }

     public class Strata
    {
        public Plant[] Herb { get; set; } = Array.Empty<Plant>();
        public Plant[] SaplingShrub { get; set; } = Array.Empty<Plant>();
        public Plant[] Tree { get; set; } = Array.Empty<Plant>();
        public Plant[] Vine { get; set;} = Array.Empty<Plant>();
    }

    public class VegetationIndicators
    {
        public bool RapidTest { get; set; } = false;
        public bool DomTest { get; set; } = false;
        public bool PrevIndex { get; set; } = false;
        public int DomWet { get; set; } = 0;
        public int DomTotal { get; set; } = 0;
        public float PrevIndexValue { get; set; } = 0f;
    }

    public class Vegetation
    {
        public bool Present { get; set; } = false;
        public bool Disturbed { get; set; } = false;
        public bool Problematic { get; set; } = false;
        public Strata Strata { get; set; } = new Strata();
        public VegetationIndicators Indicators { get; set; } = new VegetationIndicators();
        public string Remarks { get; set; } = string.Empty;
    }

    // Soils JSON
    public class RestrictiveLayer
    {
        public string? Type { get; set; }
        public int? Depth { get; set; }
    }
    public class SoilColor
    {
        public string Hue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Chroma { get; set; } = string.Empty;
    }
    public class SoilLayer
    {
        public string Id { get; set; } = string.Empty;
        public int DepthStart { get; set; } = 0;
        public int DepthEnd { get; set; } = 0;
        public string Texture { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public SoilColor MatrixColor { get; set; } = new SoilColor();
        public SoilColor RedoxColor { get; set; } = new SoilColor();
        public int MatrixPercent { get; set; } = 100;
        public int? RedoxPercent { get; set; }
        public string? RedoxType { get; set; }
        public string? RedoxLocation { get; set; }
    }
    public class Soil
    {
        public bool Present { get; set; } = false;
        public bool Disturbed { get; set; } = false;
        public bool Problematic { get; set; } = false;
        public RestrictiveLayer RestrictiveLayer { get; set; } = new RestrictiveLayer();
        public string[] Indicators { get; set; } = Array.Empty<string>();
        public string[] ProblematicIndicators { get; set; } = Array.Empty<string>();
        public string Remarks { get; set; } = string.Empty;
        public SoilLayer[] Layers { get; set; } = Array.Empty<SoilLayer>();
    }

    // Full Datapoint
    public class Datapoint
    {
        [Key, Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string ProjectId { get; set; } = string.Empty;
        public Project Project { get; set; } = new Project();

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