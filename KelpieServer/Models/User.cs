using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KelpieServer.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Admin { get; set; } = false;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
