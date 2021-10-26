using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DateOfJoining { get; set; }
        public string ProfilePicture { get; set; }
    }
}
