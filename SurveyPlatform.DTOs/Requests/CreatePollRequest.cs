namespace SurveyPlatform.DTOs.Requests
{
    public class CreatePollRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Options { get; set; }
        public int AuthorID { get; set; }
    }
}
