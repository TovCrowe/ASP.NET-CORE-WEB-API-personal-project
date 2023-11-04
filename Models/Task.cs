using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public int? UserId { get; set; }

    public virtual User? oUser { get; set; }
}
