using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models
{
    public class Option
    {
        [Key]
        public int ID { get; set; }
        public string Content { get; set; }
        public int PollID { get; set; }
    }
}
