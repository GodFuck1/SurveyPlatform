namespace SurveyPlatform.DTOs.Responses
{
    public class PollResultsResponse
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public List<OptionResult> Options { get; set; }
    }
    public class OptionResult
    {
        public int OptionId { get; set; }
        public string Content { get; set; }
        public int ResponseCount { get; set; }
    }
}
