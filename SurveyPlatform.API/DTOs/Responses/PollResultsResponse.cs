namespace SurveyPlatform.DTOs.Responses
{
    public class PollResultsResponse
    {
        public Guid PollId { get; set; }
        public string Title { get; set; }
        public List<OptionResult> Options { get; set; }
    }
    public class OptionResult
    {
        public Guid OptionId { get; set; }
        public string Content { get; set; }
        public Guid ResponseCount { get; set; }
    }
}
