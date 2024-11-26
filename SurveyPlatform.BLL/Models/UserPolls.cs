using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.BLL.Models;
public class UserPolls
{
    public Guid Id { get; set; }
    public ICollection<PollModel>? Polls { get; set; }
}
