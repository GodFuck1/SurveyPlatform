using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Requests
{
    public class UpdateUserRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
