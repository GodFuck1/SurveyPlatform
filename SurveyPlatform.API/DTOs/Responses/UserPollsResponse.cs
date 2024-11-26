namespace SurveyPlatform.DTOs.Responses;
public class UserPollsResponse
{
    public Guid Id { get; set; }
    public ICollection<PollDataResponse> Polls { get; set; }
}
