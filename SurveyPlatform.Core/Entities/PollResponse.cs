using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Core.Entities
{
    public class PollResponse
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public Poll Poll { get; set; }
        public int OptionId { get; set; }
        public PollOption Option { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
