using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DTOs.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<PollResponse>? Responses { get; set; }
        public ICollection<Poll>? Polls { get; set; }
    }
}
