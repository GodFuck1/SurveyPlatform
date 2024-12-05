namespace SurveyPlatform.DAL.Entities;
public class User
{
    public Guid Id { get; set; }
    public bool IsDeactivated { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<string> Roles { get; set; } = ["User"];
    public ICollection<PollResponse> Responses { get; set; } = [];
    public ICollection<Poll> Polls { get; set; } = [];
    public DateTime? Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; } = DateTime.UtcNow;
    public DateTime LastLoggedIn { get; set; } = DateTime.UtcNow;
}
