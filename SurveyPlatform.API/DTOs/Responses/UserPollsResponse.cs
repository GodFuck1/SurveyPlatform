using SurveyPlatform.API.DTOs.Responses;

namespace SurveyPlatform.DTOs.Responses;
public class UserPollsResponse
{
    public Guid Id { get; set; }
    public ICollection<PollDataShortResponse> Polls { get; set; }
}
