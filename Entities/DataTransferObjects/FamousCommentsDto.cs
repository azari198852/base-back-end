using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class FamousCommentsDto
    {
        public long Id { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string Comment { get; set; }
        public string CommentPic { get; set; }
    }
}
