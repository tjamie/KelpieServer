using KelpieServer.Models;

namespace KelpieServer.Mappers
{
    public class ProjectMapper
    {
        public Project MapToEntity(ProjectDto dto)
        {
            return new Project
            {
                Id = dto.Id,
                Date = dto.Date,
                Name = dto.Name,
                Applicant = dto.Applicant ?? null,
                County = dto.County ?? null,
                State = dto.State ?? null,
                Section = dto.Section ?? null,
                Region = dto.Region ?? null,
                Subregion = dto.Subregion ?? null,
                Datum = dto.Datum ?? null
            };
        }

        public void MapToEntity(ProjectDto dto, ref Project target)
        {
            target.Id = dto.Id;
            target.Date = dto.Date;
            target.Name = dto.Name;
            target.Applicant = dto.Applicant ?? null;
            target.County = dto.County ?? null;
            target.State = dto.State ?? null;
            target.Section = dto.Section ?? null;
            target.Region = dto.Region ?? null;
            target.Subregion = dto.Subregion ?? null;
            target.Datum = dto.Datum ?? null;
        }
    }
}
