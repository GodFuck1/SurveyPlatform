using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DTOs.Responses
{
    public class UserResponsesResponse
    {
        public Guid Id { get; set; }
        public ICollection<PollResultsResponse> Responses { get; set; }
    }
}
