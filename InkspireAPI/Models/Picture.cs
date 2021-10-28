using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI.Models
{
    public class Picture
    {
        public string PictureID { get; set; }
        public string UploadUserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string Url { get; set; }
    }
}
