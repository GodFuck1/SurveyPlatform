using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.DTOs.Responses;

public class UserResponsesResponse
{
    public Guid Id { get; set; }
    public ICollection<PollResponseModel> Responses { get; set; }
}
