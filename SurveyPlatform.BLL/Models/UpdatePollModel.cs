namespace SurveyPlatform.BLL.Models;
public class UpdatePollModel
{
    public Guid PollId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
