namespace SurveyPlatform.API.DTOs.Requests;

public class UpdatePollRequest
{
    public Guid PollId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
