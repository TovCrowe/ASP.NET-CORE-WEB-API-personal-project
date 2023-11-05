using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Task> oTasks { get; set; } = new List<Task>();
}
