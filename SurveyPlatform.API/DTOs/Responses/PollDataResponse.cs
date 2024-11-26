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
        public ICollection<OptionResponse> Options { get; set; }
        public ICollection<PollSubmittedResponse> Responses { get; set; }
    }
    public class OptionResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
    public class PollSubmittedResponse
    {
        public Guid Id { get; set; }
        public Guid OptionId { get; set; }
        public Guid UserId { get; set; }
    }
}
