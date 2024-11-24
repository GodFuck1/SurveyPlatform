using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DTOs.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
