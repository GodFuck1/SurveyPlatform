using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Models
{
    public class UpdatePollModel
    {
        public Guid PollId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
