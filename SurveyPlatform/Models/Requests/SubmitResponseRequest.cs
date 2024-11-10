using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class SubmitResponseRequest
    {
        [Required]
        public int OptionID { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
