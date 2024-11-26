namespace SurveyPlatform.API.DTOs.Responses
{
    public class PollDataShortResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedIn { get; set; }
    }
}
