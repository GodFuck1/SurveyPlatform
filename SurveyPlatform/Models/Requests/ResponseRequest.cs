using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class ResponseRequest
    {
        [Required]
        public int PollID { get; set; }
        [Required]
        public int OptionID { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
