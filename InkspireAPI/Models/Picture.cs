using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI.Models
{
    public class Picture
    {
        public int PictureId { get; set; }
        public string Title { get; set; }
        public int UploadUserId { get; set; }
    }
}
