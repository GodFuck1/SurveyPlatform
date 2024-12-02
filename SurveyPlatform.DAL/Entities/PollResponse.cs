namespace SurveyPlatform.DAL.Entities;
public class PollResponse
{
    public Guid Id { get; set; }
    public Poll Poll { get; set; }
    public Guid PollId { get; set; }
    public PollOption Option { get; set; }
    public Guid OptionId { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}
