using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace KelpieServer.Models
{
    public class ProjectDto
    {
        public string Id { get; set; } = string.Empty;
        public long Date { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Applicant { get; set; }
        public string? County { get; set; }
        public string? State { get; set; }
        public string? Section { get; set; }
        public string? Region { get; set; }
        public string? Subregion { get; set; }
        public string? Datum { get; set; }
        public ProjectDto(Project project)
        {
            this.Id = project.Id;
            this.Name = project.Name;
            this.Date = project.Date;
            if (project.Applicant != null)
            {
                this.Applicant = project.Applicant;
            }
            if (project.County != null)
            {
                this.County = project.County;
            }
            if (project.State != null)
            {
                this.State = project.State;
            }
            if (project.Section != null)
            {
                this.Section = project.Section;
            }
            if (project.Region != null)
            {
                this.Region = project.Region;
            }
            if (project.Subregion != null)
            {
                this.Subregion = project.Subregion;
            }
            if (project.Datum != null)
            {
                this.Datum = project.Datum;
            }
        }
        [JsonConstructor]
        public ProjectDto(string id, string name, long date, string? applicant, string? county, string? state, string? section, string? region, string? subregion, string? datum)
        {
            this.Id = id;
            this.Name = name;
            this.Date = date;
            if (applicant != null)
            {
                this.Applicant = applicant;
            }
            if (county != null)
            {
                this.County = county;
            }
            if (state != null)
            {
                this.State = state;
            }
            if (section != null)
            {
                this.Section = section;
            }
            if (region != null)
            {
                this.Region = region;
            }
            if (subregion != null)
            {
                this.Subregion = subregion;
            }
            if (datum != null)
            {
                this.Datum = datum;
            }
        }
    }
}
