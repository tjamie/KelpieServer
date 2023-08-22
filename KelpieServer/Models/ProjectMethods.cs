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
        // initialize navigation property lists
        public Project()
        {
            UserProjects = new List<UserProject>();
            Users = new List<User>();
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
