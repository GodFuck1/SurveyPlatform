namespace SurveyPlatform.DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>{ "User" };
    public ICollection<PollResponse>? Responses { get; set; }
    public ICollection<Poll>? Polls { get; set; }
    public DateTime? Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; } = DateTime.UtcNow;
    public DateTime LastLoggedIn { get; set; } = DateTime.UtcNow;
}
