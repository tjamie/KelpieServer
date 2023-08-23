namespace KelpieServer.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public bool Admin { get; set; } = false;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
