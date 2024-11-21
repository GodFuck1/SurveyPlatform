using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>{ "User" };
        public ICollection<PollResponse>? Responses { get; set; }
        public ICollection<Poll>? Polls { get; set; }
    }
}
