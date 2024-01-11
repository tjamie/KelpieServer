using KelpieServer.Models;

namespace KelpieServer.Mappers
{
    public class DatapointMapper
    {
        public Datapoint MapToEntity(DatapointDto dto)
        {
            return new Datapoint
            {
                Id = dto.Id,
                ProjectId = dto.ProjectId,
                Date = dto.Date,
                Name = dto.Name,
                Authors = dto.Authors ?? null,
                Landform = dto.Landform ?? null,
                Relief = dto.Relief ?? null,
                Slope = dto.Slope ?? null,
                SoilUnit = dto.SoilUnit ?? null,
                NormalCircumstances = dto.NormalCircumstances ?? null,
                Latitude = dto.Latitude ?? null,
                Longitude = dto.Longitude ?? null,
                NWI = dto.NWI ?? null,
                Hydrology = dto.Hydrology,
                Vegetation = dto.Vegetation,
                Soil = dto.Soil
            };
        }

        // Prevent creating a new Datapoint object
        public void MapToEntity(DatapointDto dto, ref Datapoint target)
        {
            target.Id = dto.Id;
            target.ProjectId = dto.ProjectId;
            target.Date = dto.Date;
            target.Name = dto.Name;
            target.Authors = dto.Authors ?? null;
            target.Landform = dto.Landform ?? null;
            target.Relief = dto.Relief ?? null;
            target.Slope = dto.Slope ?? null;
            target.SoilUnit = dto.SoilUnit ?? null;
            target.NormalCircumstances = dto.NormalCircumstances ?? null;
            target.Latitude = dto.Latitude ?? null;
            target.Longitude = dto.Longitude ?? null;
            target.NWI = dto.NWI ?? null;
            target.Hydrology = dto.Hydrology;
            target.Vegetation = dto.Vegetation;
            target.Soil = dto.Soil;
        }

        public DatapointDto MapToEntity(Datapoint datapoint)
        {
            return new DatapointDto
            {
                Id = datapoint.Id,
                ProjectId = datapoint.ProjectId,
                Date = datapoint.Date,
                Name = datapoint.Name,
                Authors = datapoint.Authors,
                Landform = datapoint.Landform,
                Relief = datapoint.Relief,
                Slope = datapoint.Slope,
                SoilUnit = datapoint.SoilUnit,
                NormalCircumstances = datapoint.NormalCircumstances,
                Latitude = datapoint.Latitude,
                Longitude = datapoint.Longitude,
                NWI = datapoint.NWI,
                Hydrology = datapoint.Hydrology,
                Vegetation = datapoint.Vegetation,
                Soil = datapoint.Soil
            };
        }
    }
}
