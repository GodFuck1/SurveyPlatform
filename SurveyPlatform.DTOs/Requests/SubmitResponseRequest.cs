using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Requests
{
    public class SubmitResponseRequest
    {
        public int OptionID { get; set; }
        public int UserID { get; set; }
    }
}
