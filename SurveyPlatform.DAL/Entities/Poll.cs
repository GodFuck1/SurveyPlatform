namespace SurveyPlatform.DAL.Entities;
public class Poll
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid AuthorID { get; set; }
    public User Author { get; set; }
    public ICollection<PollOption> Options { get; set; }
    public ICollection<PollResponse> Responses { get; set; }
}
