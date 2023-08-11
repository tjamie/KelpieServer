using System.Data;
using System.Text.Json.Serialization;

 namespace KelpieServer.Models
{
    public class DatapointDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public long Date { get; set; }
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
        public Hydrology Hydrology { get; set; } = new Hydrology();
        public Vegetation Vegetation { get; set; } = new Vegetation();
        public Soil Soil { get; set; } = new Soil();
            
        //public DatapointDto(Datapoint datapoint)
        //{
        //    this.Id = datapoint.Id;
        //    this.ProjectId = datapoint.ProjectId;
        //    this.Date = datapoint.Date;
        //    this.Name = datapoint.Name;
        //    this.Authors = datapoint.Authors;
        //    this.Landform = datapoint.Landform;
        //    this.Relief = datapoint.Relief;
        //    this.Slope = datapoint.Slope;
        //    this.SoilUnit = datapoint.SoilUnit;
        //    this.NormalCircumstances = datapoint.NormalCircumstances;
        //    this.Latitude = datapoint.Latitude;
        //    this.Longitude = datapoint.Longitude;
        //    this.NWI = datapoint.NWI;
        //    this.Hydrology = datapoint.Hydrology;
        //    this.Vegetation = datapoint.Vegetation;
        //    this.Soil = datapoint.Soil;
        //}

        //JSON deserialization
        public DatapointDto()
        {
            this.Hydrology = new Hydrology();
            this.Vegetation = new Vegetation();
            this.Soil = new Soil();
        }
    }
}
