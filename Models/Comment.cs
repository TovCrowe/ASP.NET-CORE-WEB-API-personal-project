using System;
using System.Collections.Generic;

namespace WebApplication7.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Content { get; set; } 

    public int? UserId { get; set; }

    public int? TaskId { get; set; }

    public virtual Task? oTask { get; set; }

    public virtual User? oUser { get; set; }
}
