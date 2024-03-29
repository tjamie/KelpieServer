﻿using System.ComponentModel.DataAnnotations;
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

        // Many-to-many for assigned projects
        public List<UserProject> UserProjects { get; } = new();
        public List<Project> Projects { get; } = new();

        // initialize navigation property lists
        public User()
        {
            UserProjects = new List<UserProject>();
            Projects = new List<Project>();
        }
    }
}
