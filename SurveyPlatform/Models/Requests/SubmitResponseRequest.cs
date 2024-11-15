using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class SubmitResponseRequest
    {
        public int OptionID { get; set; }
        public int UserID { get; set; }
    }
}
