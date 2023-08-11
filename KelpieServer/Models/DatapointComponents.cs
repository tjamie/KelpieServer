using System.Text.Json.Serialization;

namespace KelpieServer.Models
{
    // Hydrology JSON
    public class WaterPresence
    {
        public float? Depth { get; set; }
        public bool Present { get; set; } = false;

        public WaterPresence()
        {
            Depth = null;
            Present = false;
        }

        [JsonConstructor]
        public WaterPresence(float? depth, bool present)
        {
            this.Depth = depth ?? null;
            this.Present = present;
        }
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

        public Hydrology()
        {
            Present = false;
            Disturbed = false;
            Problematic = false;
            SurfaceWater = new WaterPresence();
            WaterTable = new WaterPresence();
            Saturation = new WaterPresence();
            PrimaryIndicators = Array.Empty<string>();
            SecondaryIndicators = Array.Empty<string>();
            Remarks = "";
        }

        [JsonConstructor]
        public Hydrology(bool present, bool disturbed, bool problematic, WaterPresence surfaceWater, WaterPresence waterTable, WaterPresence saturation, string[] primaryIndicators, string[] secondaryIndicators, string remarks)
        {
            this.Present = present;
            this.Disturbed = disturbed;
            this.Problematic = problematic;
            this.SurfaceWater = surfaceWater;
            this.WaterTable = waterTable;
            this.Saturation = saturation;
            this.PrimaryIndicators = primaryIndicators;
            this.SecondaryIndicators = secondaryIndicators;
            this.Remarks = remarks;
        }
    }

    // Vegetation JSON
    public class Plant
    {
        public string Id { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Indicator { get; set; } = string.Empty;
        public int Cover { get; set; } = 0;
        public bool Dominant { get; set; } = false;

        public Plant()
        {
            this.Id = "";
            this.Species = "";
            this.Indicator = "";
            this.Cover = 0;
            this.Dominant = false;
        }

        [JsonConstructor]
        public Plant(string id, string species, string indicator, int cover, bool dominant)
        {
            this.Id = id;
            this.Species = species;
            this.Indicator = indicator;
            this.Cover = cover;
            this.Dominant = dominant;
        }
    }

    public class Strata
    {
        public Plant[] Herb { get; set; } = Array.Empty<Plant>();
        public Plant[] SaplingShrub { get; set; } = Array.Empty<Plant>();
        public Plant[] Tree { get; set; } = Array.Empty<Plant>();
        public Plant[] Vine { get; set; } = Array.Empty<Plant>();

        public Strata()
        {
            this.Herb = Array.Empty<Plant>();
            this.SaplingShrub = Array.Empty<Plant>();
            this.Tree = Array.Empty<Plant>();
            this.Vine = Array.Empty<Plant>();
        }

        [JsonConstructor]
        public Strata(Plant[] herb, Plant[] saplingShrub, Plant[] tree, Plant[] vine)
        {
            this.Herb = herb;
            this.SaplingShrub = saplingShrub;
            this.Tree = tree;
            this.Vine = vine;
        }
    }

    public class VegetationIndicators
    {
        public bool RapidTest { get; set; } = false;
        public bool DomTest { get; set; } = false;
        public bool PrevIndex { get; set; } = false;
        public int DomWet { get; set; } = 0;
        public int DomTotal { get; set; } = 0;
        public float PrevIndexValue { get; set; } = 0f;

        public VegetationIndicators()
        {
            this.RapidTest = false;
            this.DomTest = false;
            this.PrevIndex = false;
            this.DomWet = 0;
            this.DomTotal = 0;
            this.PrevIndexValue = 0f;
        }

        [JsonConstructor]
        public VegetationIndicators(bool rapidTest, bool domTest, bool prevIndex, int domWet, int domTotal, float prevIndexValue)
        {
            this.RapidTest = rapidTest;
            this.DomTest = domTest;
            this.PrevIndex = prevIndex;
            this.DomWet = domWet;
            this.DomTotal = domTotal;
            this.PrevIndexValue = prevIndexValue;
        }
    }

    public class Vegetation
    {
        public bool Present { get; set; } = false;
        public bool Disturbed { get; set; } = false;
        public bool Problematic { get; set; } = false;
        public Strata Strata { get; set; } = new Strata();
        public VegetationIndicators Indicators { get; set; } = new VegetationIndicators();
        public string Remarks { get; set; } = string.Empty;

        public Vegetation()
        {
            this.Present = false;
            this.Disturbed = false;
            this.Problematic = false;
            this.Strata = new Strata();
            this.Indicators = new VegetationIndicators();
            this.Remarks = string.Empty;
        }

        [JsonConstructor]
        public Vegetation(bool present, bool disturbed, bool problematic, Strata strata, VegetationIndicators indicators, string remarks)
        {
            this.Present = present;
            this.Disturbed = disturbed;
            this.Problematic = problematic;
            this.Strata = strata;
            this.Indicators = indicators;
            this.Remarks = remarks;
        }
    }

    // Soils JSON
    public class RestrictiveLayer
    {
        public string? Type { get; set; }
        public int? Depth { get; set; }

        public RestrictiveLayer()
        {
            this.Type = "";
            this.Depth = null;
        }

        [JsonConstructor]
        public RestrictiveLayer(string? type, int? depth)
        {
            this.Type = type ?? "";
            this.Depth = depth ?? null;
        }
    }
    public class SoilColor
    {
        public string Hue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Chroma { get; set; } = string.Empty;

        public SoilColor()
        {
            this.Hue = string.Empty;
            this.Value = string.Empty;
            this.Chroma = string.Empty;
        }

        [JsonConstructor]
        public SoilColor(string hue, string value, string chroma)
        {
            this.Hue = hue;
            this.Value = value;
            this.Chroma = chroma;
        }
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

        public SoilLayer()
        {
            this.Id = string.Empty;
            this.DepthStart = 0;
            this.DepthEnd = 0;
            this.Texture = string.Empty;
            this.Remarks = string.Empty;
            this.MatrixColor = new SoilColor();
            this.RedoxColor = new SoilColor();
            this.MatrixPercent = 100;
            this.RedoxPercent = null;
            this.RedoxType = null;
            this.RedoxLocation = null;
        }

        [JsonConstructor]
        public SoilLayer(string id, int depthStart, int depthEnd, string texture, string remarks, SoilColor matrixColor, SoilColor redoxColor, int matrixPercent, int? redoxPercent, string? redoxType, string? redoxLocation)
        {
            this.Id = id;
            this.DepthStart = depthStart;
            this.DepthEnd = depthEnd;
            this.Texture = texture;
            this.Remarks = remarks;
            this.MatrixColor = matrixColor;
            this.RedoxColor = redoxColor;
            this.MatrixPercent = matrixPercent;
            this.RedoxPercent = redoxPercent ?? null;
            this.RedoxType = redoxType ?? null;
            this.RedoxLocation = redoxLocation ?? null;
        }
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

        public Soil()
        {
            this.Present = false;
            this.Disturbed = false;
            this.Problematic = false;
            this.RestrictiveLayer = new RestrictiveLayer();
            this.Indicators = Array.Empty<string>();
            this.ProblematicIndicators = Array.Empty<string>();
            this.Remarks = string.Empty;
            this.Layers = Array.Empty<SoilLayer>();
        }

        [JsonConstructor]
        public Soil(bool present, bool disturbed, bool problematic, RestrictiveLayer restrictiveLayer, string[] indicators, string[] problematicIndicators, string remarks, SoilLayer[] layers)
        {
            this.Present = present;
            this.Disturbed = disturbed;
            this.Problematic = problematic;
            this.RestrictiveLayer = restrictiveLayer;
            this.Indicators = indicators;
            this.ProblematicIndicators = problematicIndicators;
            this.Remarks = remarks;
            this.Layers = layers;
        }
    }
}
