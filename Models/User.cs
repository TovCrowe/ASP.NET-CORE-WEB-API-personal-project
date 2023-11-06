using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Comment> oComments { get; set; } = new List<Comment>();

    public virtual ICollection<Project> oProjects { get; set; } = new List<Project>();

    public virtual ICollection<Task> oTasks { get; set; } = new List<Task>();
}
