namespace SurveyPlatform.BLL.Models;
public class PollModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<PollOptionModel> Options { get; set; }
    public ICollection<PollResponseModel> Responses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid AuthorID { get; set; }
}
