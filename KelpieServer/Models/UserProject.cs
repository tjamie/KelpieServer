namespace KelpieServer.Models
{
    public class UserProject
    {
        // Keep track of which users are authorized to work on which project
        public int UserId { get; set; }
        public string ProjectId { get; set; }
        public User User { get; set; } = null!;
        public Project Project { get; set; } = null!;
    }
}
