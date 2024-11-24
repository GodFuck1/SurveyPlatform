using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Responses
{
    public class PollDataResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedIn { get; set; }
        public Guid AuthorID { get; set; }
        public List<OptionResponse> Options { get; set; }
    }
    public class OptionResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}
