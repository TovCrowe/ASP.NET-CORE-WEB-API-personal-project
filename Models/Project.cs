using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string NameProject { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public virtual User? oUser { get; set; }
}
