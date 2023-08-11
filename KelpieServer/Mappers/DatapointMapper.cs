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
    }
}
