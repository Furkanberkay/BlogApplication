using System;

namespace BlogApp.Entity;

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public DateTime PublishOn { get; set; }
    public bool IsActive { get; set; }
    public int UserId { get; set; }

    public User? User { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
