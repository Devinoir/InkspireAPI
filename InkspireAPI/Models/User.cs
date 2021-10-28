using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsArtist { get; set; }
        public string Pronouns { get; set; }
        public DateTime JoinDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}
