namespace SurveyPlatform.DTOs.Responses;

public class UserResponsesResponse
{
    public Guid Id { get; set; }
    public ICollection<PollShortResponseModel> Responses { get; set; }
}
public class PollShortResponseModel
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }
}
