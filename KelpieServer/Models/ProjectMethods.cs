using System.Text.Json;

namespace KelpieServer.Models
{
    public partial class Project
    {
        public Project(ProjectDto project)
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
        public Project()
        {
            this.Id = "";
            this.Name = "";
            this.Date = 0;
            this.Applicant = null;
            this.County = null;
            this.State = null;
            this.Section = null;
            this.Region = null;
            this.Subregion = null;
            this.Datum = null;
        }
        public void Update(ProjectDto project)
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
    }    
}
