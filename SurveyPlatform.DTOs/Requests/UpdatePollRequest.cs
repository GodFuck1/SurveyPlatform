using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Requests
{
    public class UpdatePollRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
