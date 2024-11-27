using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.BLL.Models;
public class UserPollsModel
{
    public Guid Id { get; set; }
    public ICollection<PollModel>? Polls { get; set; }
}
