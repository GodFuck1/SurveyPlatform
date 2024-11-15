using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class UpdateUserRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
